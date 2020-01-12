using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace CalorieTracker.Backend.Tests.Tests
{
	[TestClass]
	public class StatisticsTests
	{
		[TestMethod]
		public void StatisticsTest()
		{
			var user = new LogonModel { UserName = Guid.NewGuid().ToString(), Password = "123" };
			var api = new Api();
			api.Account.Register(user);
			api.Account.Logon(user);

			var date1 = new DateTime(2016, 3, 15);
			api.Entries.Save(new Entry { Date = date1.Add(new TimeSpan(9, 0, 0)), Calories = 100, Comment = "breakfast" });
			api.Entries.Save(new Entry { Date = date1.Add(new TimeSpan(13, 0, 0)), Calories = 200, Comment = "lunch" });

			var date2 = new DateTime(2016, 3, 16);
			api.Entries.Save(new Entry { Date = date2.Add(new TimeSpan(9, 0, 0)), Calories = 150, Comment = "breakfast" });
			api.Entries.Save(new Entry { Date = date2.Add(new TimeSpan(19, 0, 0)), Calories = 200, Comment = "dinner" });

			var date3 = new DateTimeOffset(2016, 3, 17, 0, 0, 0, TimeSpan.FromHours(4));
			api.Entries.Save(new Entry { Date = date3.Add(new TimeSpan(23, 59, 59)), Calories = 100, Comment = "high bound" });
			api.Entries.Save(new Entry { Date = date3.Add(new TimeSpan(0, 0, 0)), Calories = 300, Comment = "lower bound" });

			var stats = api.Statistics.List();
			stats.ToList().ForEach(s => Console.WriteLine(s.Date + " = " + s.Sum));
			stats.Count().ShouldEqual(3);
			// order is important
			stats[0].Date.ShouldEqual(date1);
			stats[0].Sum.ShouldEqual(300);
			stats[1].Date.ShouldEqual(date2);
			stats[1].Sum.ShouldEqual(350);
			stats[2].Date.ShouldEqual(date3);
			stats[2].Sum.ShouldEqual(400);
		}
	}
}
