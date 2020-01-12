using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using CalorieTracker.Web.App_Start;

namespace CalorieTracker.Web
{
	/// <summary>
	/// MVC application.
	/// </summary>
	public class Global : HttpApplication
	{
		/// <summary>
		/// MVC application startup.
		/// </summary>
		protected void Application_Start(object sender, EventArgs e)
		{
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterScriptBundles(BundleTable.Bundles);
		}
	}
}