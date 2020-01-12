using System.Web.Mvc;

namespace CalorieTracker.Web.Controllers
{
	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			return View();
		}
	}
}