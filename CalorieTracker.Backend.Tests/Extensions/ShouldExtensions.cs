using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;

namespace CalorieTracker.Backend.Tests.Extensions
{
	/// <summary>
	/// Extensions for testing.
	/// </summary>
	static class ShouldExtensions
	{
		/// <summary>
		/// Checks that enumeration contains specified element.
		/// </summary>
		public static void ShouldContain<T>(this IEnumerable<T> items, Func<T, bool> predicate)
		{
			items.Any(predicate).ShouldBeTrue();
		}

		/// <summary>
		/// Checks that enumeration does not contain specified element.
		/// </summary>
		public static void ShouldNotContain<T>(this IEnumerable<T> items, Func<T, bool> predicate)
		{
			items.Any(predicate).ShouldBeFalse();
		}
	}
}
