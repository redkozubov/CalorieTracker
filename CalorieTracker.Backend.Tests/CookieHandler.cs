using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CalorieTracker.Backend.Tests
{
	/// <summary>
	/// Persists cookies in HttpClient.
	/// </summary>
	class CookieHandler : DelegatingHandler
	{
		public CookieContainer CookieContainer { get; set; }

		private HttpResponseMessage handleResponse(HttpRequestMessage request, HttpResponseMessage response)
		{
			var cookieHeaders = response.Headers.Where(pair => pair.Key == "Set-Cookie");
			foreach (var value in cookieHeaders.SelectMany(header => header.Value))
			{
				CookieContainer.SetCookies(request.RequestUri, value);
			}
			return response;
		}

		private void setRequestHeaders(HttpRequestMessage request)
		{
			string cookieHeader = CookieContainer.GetCookieHeader(request.RequestUri);
			if (!string.IsNullOrEmpty(cookieHeader))
			{
				request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
			}
		}

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			setRequestHeaders(request);
			return base.SendAsync(request, cancellationToken)
				.ContinueWith(t => handleResponse(request, t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
		}
	}
}
