using System.Net.Http;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Tests.Extensions;

namespace CalorieTracker.Backend.Tests.Wrappers
{
	/// <summary>
	/// Represents a wrapper around statistics api.
	/// </summary>
	class Statistics
	{
		private HttpClient _client;

		public Statistics(HttpClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Returns list of entries.
		/// </summary>
		public DailySum[] List()
		{
			return _client.GetAsync("/api/statistics").ReadResponse<DailySum[]>();
		}
	}
}
