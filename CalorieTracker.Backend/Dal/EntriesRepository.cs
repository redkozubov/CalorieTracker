using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Extensions;
using CalorieTracker.Backend.Models;

namespace CalorieTracker.Backend.Dal
{
	/// <summary>
	/// Contains business logic for entries.
	/// </summary>
	class EntriesRepository
	{
		private readonly CalorieTrackerContext _context;
		private int _userId;

		/// <summary>
		/// Initializes a new instance of entries repository.
		/// </summary>
		public EntriesRepository(int userId)
		{
			_userId = userId;
			_context = new CalorieTrackerContext();
		}

		private IQueryable<Entry> getEntries()
		{
			return _context.Entries.Where(e => e.User.Id == _userId);
		}

		/// <summary>
		/// Return all entries available to user.
		/// </summary>
		public async Task<Entry[]> ListAsync(DateTime? dateFrom, DateTime? dateTo, TimeSpan? timeFrom, TimeSpan? timeTo)
		{
			var entries = getEntries();
			if (dateFrom.HasValue)
			{
				entries = entries.Where(e => DbFunctions.TruncateTime(e.Date) >= dateFrom);
			}
			if (dateTo.HasValue)
			{
				entries = entries.Where(e => DbFunctions.TruncateTime(e.Date) <= dateTo);
			}
			if (timeFrom.HasValue)
			{
				var seconds = (int)timeFrom.Value.TotalSeconds;
				entries = entries.Where(e => e.Date >= DbFunctions.AddSeconds(DbFunctions.TruncateTime(e.Date), seconds));
			}
			if (timeTo.HasValue)
			{
				var seconds = (int)timeTo.Value.TotalSeconds;
				entries = entries.Where(e => e.Date <= DbFunctions.AddSeconds(DbFunctions.TruncateTime(e.Date), seconds));
			}
			return await entries.OrderBy(e => e.Date).ToArrayAsync();
		}

		/// <summary>
		/// Get single entry.
		/// </summary>
		public async Task<Entry> GetAsync(int id)
		{
			return await getEntries().SingleOrNotFoundAsync(e => e.Id == id, "Entry not found");
		}

		/// <summary>
		/// Creates or updates entry.
		/// </summary>
		public async Task<Entry> SaveAsync(Entry entry)
		{
			if (entry.Id == 0)
			{
				var user = await _context.Users.SingleOrNotFoundAsync(u => u.Id == _userId, "Current user not found");
				entry.User = user;
				_context.Entries.Add(entry);
			}
			else
			{
				var entryInDb = await getEntries()
					.Include(e => e.User)
					.SingleOrNotFoundAsync(e => e.Id == entry.Id, "Entry not found");
				_context.Entry(entryInDb).CurrentValues.SetValues(entry);
				entry = entryInDb;
			}
			await _context.SaveChangesAsync();
			return entry;
		}

		/// <summary>
		/// Deletes entry.
		/// </summary>
		public async Task DeleteAsync(int id)
		{
			var entry = await getEntries().SingleOrNotFoundAsync(e => e.Id == id, "Entry not found");
			_context.Entries.Remove(entry);
			await _context.SaveChangesAsync();
		}

		/// <summary>
		/// Returns daily consumption statistics.
		/// </summary>
		/// <returns></returns>
		public async Task<DailySum[]> Statistics()
		{
			var query =
				from entry in getEntries()
				group entry by DbFunctions.TruncateTime(entry.Date) into g
				select new DailySum { Date = g.Key, Sum = g.Sum(e => (long)e.Calories) };
			query = query.OrderBy(s => s.Date);
			return await query.ToArrayAsync();
		}

		public async Task<Entry[]> GetEntriesByComment(string comment, int limit)
		{
			var data = getEntries().Where(e => e.Comment.StartsWith(comment)).Take(limit).ToArray();
			return await Task.FromResult(data);
		}
	}
}
