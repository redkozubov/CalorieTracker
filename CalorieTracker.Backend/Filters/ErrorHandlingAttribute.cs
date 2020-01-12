using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using CalorieTracker.Backend.Exceptions;
using NLog;

namespace CalorieTracker.Backend.Filters
{
	/// <summary>
	/// Converts .net exceptions to http response.
	/// </summary>
	/// <remarks>
	/// ObjectNotFoundException - 404
	/// BadRequestException - 400
	/// </remarks>
	class ErrorHandlingAttribute : ExceptionFilterAttribute
	{
		/// <summary>
		/// Exceptions that are converted to HTTP error codes.
		/// </summary>
		private Dictionary<Type, HttpStatusCode> _handledExceptionTypes = new Dictionary<Type, HttpStatusCode>
		{
			{ typeof(ObjectNotFoundException), HttpStatusCode.NotFound },
			{ typeof(BadRequestException), HttpStatusCode.BadRequest },
			{ typeof(UnauthorizedAccessException), HttpStatusCode.Forbidden }
		};

		/// <summary>
		/// Handles exceptions.
		/// </summary>
		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			var exception = actionExecutedContext.Exception;
			if (exception == null)
			{
				return;
			}
			var exceptionType = exception.GetType();
			if (_handledExceptionTypes.ContainsKey(exceptionType))
			{
				var statusCode = _handledExceptionTypes[exceptionType];
				actionExecutedContext.Response = actionExecutedContext.Request
					.CreateErrorResponse(statusCode, exception.Message);
			}
			else
			{
				// log unhandled exceptions
				var log = LogManager.GetLogger(actionExecutedContext.ActionContext.ControllerContext.Controller.GetType().Name);
				var request = actionExecutedContext.Request.Method.Method + " " + actionExecutedContext.Request.RequestUri;
				log.Error(exception, "Error executing request " + request);
			}
		}
	}
}
