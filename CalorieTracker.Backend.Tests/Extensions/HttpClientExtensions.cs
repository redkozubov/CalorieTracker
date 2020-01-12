using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Should;

namespace CalorieTracker.Backend.Tests.Extensions
{
	/// <summary>
	/// Extensions for handling HttpClient responses.
	/// </summary>
	static class HttpClientExtensions
	{
		/// <summary>
		/// Reads response and returns object from api.
		/// </summary>
		public static T ReadResponse<T>(this Task<HttpResponseMessage> responseTask, 
			HttpStatusCode expectedCode = HttpStatusCode.OK)
		{
			var response = responseTask.Result;
			try
			{
				response.StatusCode.ShouldEqual(expectedCode);
				return response.Content.ReadAsAsync<T>().Result;
			}
			catch
			{
				// output response that causes error
				Console.WriteLine(response.Content.ReadAsStringAsync().Result);
				throw;
			}
		}

		/// <summary>
		/// Reads response without content.
		/// </summary>
		public static void ReadResponse(this Task<HttpResponseMessage> responseTask,
			HttpStatusCode expectedCode = HttpStatusCode.NoContent)
		{
			responseTask.ReadResponse<object>(expectedCode);
		}
	}
}
