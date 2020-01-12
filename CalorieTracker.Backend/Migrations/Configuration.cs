using System.Data.Entity.Migrations;
using System.Linq;
using CalorieTracker.Backend.Dal;
using CalorieTracker.Backend.Models;

namespace CalorieTracker.Backend.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<CalorieTrackerContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}

		/// <summary>
		/// Adds first user admin / admin.
		/// </summary>
		protected override void Seed(CalorieTrackerContext context)
		{
			if (!context.Users.Any())
			{
				// hash for password "admin"
				var password = @"AA2rbneApsi9C/XQo1ekf9zTfUav0VgWTaDqO2u0bXQlF4RDLg9kxVhxZLsu3r71wQ==";
				// user with logon admin / admin
				var user = new User { IsAdmin = true, UserName = "admin", PasswordHash = password };
				context.Users.Add(user);
				context.SaveChanges();
			}
		}
	}
}
