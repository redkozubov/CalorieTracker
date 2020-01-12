using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity;

namespace CalorieTracker.Backend.Extensions
{
	/// <summary>
	/// Extensions for HttpRequestContext.
	/// </summary>
	static class RequestContextExtensions
	{
		/// <summary>
		/// Gets id of current authorized user from claims.
		/// </summary>
		public static int? GetCurrentUserId(this HttpRequestContext context)
		{
			return context?.Principal?.Identity?.GetUserId<int>();
		}
	}
}
