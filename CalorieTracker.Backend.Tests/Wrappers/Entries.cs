using CalorieTracker.Backend.Models;
using System.Net.Http;
using System;
using Should;
using System.Net;
using System.Threading.Tasks;
using CalorieTracker.Backend.Tests.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace CalorieTracker.Backend.Tests.Wrappers
{
	/// <summary>
	/// Represents a wrapper around entries api.
	/// </summary>
	class Entries
	{
		private readonly HttpClient _client;

		public Entries(HttpClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Returns list of entries.
		/// </summary>
		public Entry[] List(DateTime? dateFrom = null, DateTime? dateTo = null,
			TimeSpan? timeFrom = null, TimeSpan? timeTo = null)
		{
			var parameters = new[]
				{
					new { name = nameof(dateFrom), value = dateFrom?.ToString("yyyy-MM-dd") },
					new { name = nameof(dateTo), value = dateTo?.ToString("yyyy-MM-dd") },
					new { name = nameof(timeFrom), value = timeFrom?.ToString() },
					new { name = nameof(timeTo), value = timeTo?.ToString() }
				}
				.Where(p => p.value != null)
				.Select(p => $"{p.name}={p.value}")
				.ToArray();
			var query = string.Join("&", parameters);
            return _client.GetAsync("/api/entries?"+query).ReadResponse<Entry[]>();
		}

		public void ListWithError(HttpStatusCode expectedCode)
		{
			_client.GetAsync("/api/entries").ReadResponse(expectedCode);
		}

		/// <summary>
		/// Loads single entry.
		/// </summary>
		public Entry Get(int id)
		{
			return _client.GetAsync("/api/entries/" + id).ReadResponse<Entry>();
		}
		
		/// <summary>
		/// Searches for a given number of entities by comment.
		/// </summary>
		public Entry[] Search(string comment, int limit)
		{
			var url = $"/api/entries/search?comment={comment}&limit={limit}";
			return _client.GetAsync(url).ReadResponse<Entry[]>();
		}

		/// <summary>
		/// Saves an entry.
		/// </summary>
		public Entry Save(Entry entry)
		{
			return _client.PostAsJsonAsync("/api/entries", entry).ReadResponse<Entry>();
		}

		/// <summary>
		/// Saves an entry.
		/// </summary>
		public void SaveWithError(Entry entry, HttpStatusCode expectedCode)
		{
			_client.PostAsJsonAsync("/api/entries", entry).ReadResponse(expectedCode);
		}

		/// <summary>
		/// Deletes an entry expecting custom error code.
		/// </summary>
		public void Delete(int id, HttpStatusCode expectedCode = HttpStatusCode.NoContent)
		{
			_client.DeleteAsync("/api/entries/" + id).ReadResponse(expectedCode);
		}
	}
}
