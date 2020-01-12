using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Backend.Controllers.Models
{
	/// <summary>
	/// Represents daily calories consumption.
	/// </summary>
	public class DailySum
	{
		/// <summary>
		/// Date.
		/// </summary>
		public DateTimeOffset? Date { get; set; }

		/// <summary>
		/// Number of calories.
		/// </summary>
		public long? Sum { get; set; }
	}
}
