using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace CalorieTracker.Web.App_Start
{
	public static class BundleConfig
	{
		public static void RegisterScriptBundles(BundleCollection bundles)
		{
			var angularBundle = new ScriptBundle("~/bundles/scripts/angular")
				.Include("~/Scripts/angular.js")
				.Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js")
				.Include("~/Scripts/angular-route.js")
				.Include("~/Scripts/angular-messages.js")
				.Include("~/Scripts/angular-toastr.tpls.js");
			bundles.Add(angularBundle);

			var appScriptBundle = new ScriptBundle("~/bundles/scripts/app")
				.Include("~/app/app.js")
				.IncludeDirectory("~/app/", "*.js", searchSubdirectories: true);
			bundles.Add(appScriptBundle);

			var styleBundle = new StyleBundle("~/bundles/styles")
				.Include("~/Content/bootstrap.css")
				.Include("~/Content/font-awesome.css")
				.Include("~/Content/angular-toastr.css");
			bundles.Add(styleBundle);
		}
	}
}
