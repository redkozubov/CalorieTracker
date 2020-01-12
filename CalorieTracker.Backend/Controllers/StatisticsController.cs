using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Dal;
using Microsoft.AspNet.Identity;

namespace CalorieTracker.Backend.Controllers
{
	/// <summary>
	/// Provides daily calorie consumption statistics.
	/// </summary>
	public class StatisticsController : ApiController
	{
		/// <summary>
		/// Gets consumption statistics.
		/// </summary>
		[HttpGet]
		public async Task<DailySum[]> List()
		{
			var repository = new EntriesRepository(RequestContext.Principal.Identity.GetUserId<int>());
			return await repository.Statistics();
		}
	}
}
