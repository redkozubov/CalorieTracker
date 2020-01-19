﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using CalorieTracker.Backend.Dal;
using CalorieTracker.Backend.Filters;
using CalorieTracker.Backend.Models;
using Microsoft.AspNet.Identity;

namespace CalorieTracker.Backend.Controllers
{
	/// <summary>
	/// Provides access to entries.
	/// </summary>
	[ErrorHandling]
	public class EntriesController : ApiController
	{
		private EntriesRepository getRepository()
		{
			return new EntriesRepository(RequestContext.Principal.Identity.GetUserId<int>());
		}

		/// <summary>
		/// Returns list of entries.
		/// </summary>
		[HttpGet]
		public async Task<Entry[]> List(DateTime? dateFrom = null, DateTime? dateTo = null, 
			TimeSpan? timeFrom = null, TimeSpan? timeTo = null)
		{
			return await getRepository().ListAsync(dateFrom, dateTo, timeFrom, timeTo);
		}

		/// <summary>
		/// Returns single entry.
		/// </summary>
		/// <param name="id">Entry identifier.</param>
		[HttpGet]
		public async Task<Entry> Get(int id)
		{
			return await getRepository().GetAsync(id);
		}


		/// <summary>
		/// Searches for a given number of entities by comment.
		/// </summary>
		/// <param name="comment">The substring with which the comment should begin.</param>
		/// <param name="limit">Number of entities.</param>
		/// <returns>Given number of entities.</returns>
		[HttpGet]
		[Route("api/entries/search")]
		public async Task<Entry[]> Search(string comment, int limit = 5)
		{
			return await getRepository().GetEntriesByComment(comment, limit);
		}

		/// <summary>
		/// Creates new or updates exising entry.
		/// </summary>
		/// <param name="entry">Entry to create or update.</param>
		/// <returns>Save entry.</returns>
		[HttpPost]
		public async Task<Entry> Save(Entry entry)
		{
			return await getRepository().SaveAsync(entry);
		}

		/// <summary>
		/// Deletes entry.
		/// </summary>
		[HttpDelete]
		public async Task Delete(int id)
		{
			await getRepository().DeleteAsync(id);
		}
	}
}
