using CalorieTracker.Backend.Models;
using System.Data.Entity;
using CalorieTracker.Backend.Migrations;

namespace CalorieTracker.Backend.Dal
{
	/// <summary>
	/// Represents a DbContext for the entire project.
	/// </summary>
	public class CalorieTrackerContext : DbContext
	{
		/// <summary>
		/// Initializes a new instance of calorie tracker context.
		/// </summary>
		public CalorieTrackerContext()
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<CalorieTrackerContext, Configuration>());
		}

		/// <summary>
		/// Calorie records.
		/// </summary>
		public DbSet<Entry> Entries { get; set; }

		/// <summary>
		/// Users.
		/// </summary>
		public DbSet<User> Users { get; set; }
	}
}
