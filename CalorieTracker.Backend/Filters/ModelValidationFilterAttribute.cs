using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CalorieTracker.Backend.Filters
{
	/// <summary>
	/// Verifies request model and returns 400 if model is invalid.
	/// </summary>
	public class ModelValidationFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			base.OnActionExecuting(actionContext);
			var modelState = actionContext.ModelState;
			if (!modelState.IsValid)
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
			}
		}
	}
}