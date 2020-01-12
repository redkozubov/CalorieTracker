using System;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CalorieTracker.Backend.Models;

namespace CalorieTracker.Backend.Extensions
{
	/// <summary>
	/// Extensions for IQueryable.
	/// </summary>
	static class QueryableExtensions
	{
		/// <summary>
		/// Finds an object with specified Id.
		/// </summary>
		/// <exception cref="ObjectNotFoundException">If object with such id is not found.</exception>
		public static async Task<T> SingleOrNotFoundAsync<T>(this IQueryable<T> query, 
			Expression<Func<T, bool>> predicate, string errorText = null)
		{
			var result = await query.SingleOrDefaultAsync(predicate);
			if (result == null)
			{
				throw new ObjectNotFoundException(errorText ?? "Object not found");
			}
			return result;
		}
	}
}
