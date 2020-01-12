using System;
using System.Web.Http;
using CalorieTracker.Backend.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;
using Owin;

namespace CalorieTracker.Backend
{
	/// <summary>
	/// Startup class for Owin initialization.
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Include debug information in API responses.
		/// </summary>
		private readonly bool _debug;

		public Startup() : this(false)
		{
		}

		public Startup(bool debug)
		{
			_debug = debug;
		}

		/// <summary>
		/// Configures Web API routing and other parameters.
		/// </summary>
		public HttpConfiguration GetHttpConfiguration()
		{
			var config = new HttpConfiguration();
			config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
			config.Filters.Add(new ErrorHandlingAttribute());
			config.Filters.Add(new AuthorizeAttribute());
			config.Filters.Add(new ModelValidationFilterAttribute());

			if (_debug)
			{
				config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
				// for readability
				config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
			}

			// for account routes
			config.MapHttpAttributeRoutes();
			// for REST API routes
			config.Routes.MapHttpRoute(
				name: "DefaultApiRoute",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			return config;
		}

		/// <summary>
		/// Configures Owin application.
		/// </summary>
		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				ExpireTimeSpan = TimeSpan.FromDays(1),
				SlidingExpiration = true,
			});

			var httpConfiguration = GetHttpConfiguration();
			app.UseWebApi(httpConfiguration);
		}
	}
}
