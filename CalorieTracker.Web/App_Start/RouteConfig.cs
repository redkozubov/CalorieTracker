using System.Web.Mvc;
using System.Web.Routing;

namespace CalorieTracker.Web.App_Start
{
	/// <summary>
	/// Route configuration for MVC application.
	/// </summary>
	public class RouteConfig
	{
		/// <summary>
		/// Registers single route for HomeController.
		/// </summary>
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.MapRoute(
				name: "Default",
				url: "",
				defaults: new { controller = "Home", action = "Index" }
			);
		}
	}
}
