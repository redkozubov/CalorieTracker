using System;
using System.Net;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Models;
using CalorieTracker.Backend.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace CalorieTracker.Backend.Tests.Tests
{
	[TestClass]
	public class UsersTests
	{
		[TestMethod]
		public void CreateReadUpdateDeleteUser()
		{
			var api = new Api();
			api.Account.Logon(new LogonModel { UserName = "admin", Password = "admin" });

			// create new user
			var user = new User
			{
				UserName = "randomUser",
				IsAdmin = false,
				DailyGoal = 100
			};

			var savedUser = api.Users.Save(user);
			// make sure non-empty id is returned
			savedUser.Id.ShouldNotEqual(0);
			user.Id = savedUser.Id;
			checkUsersAreEqual(user, savedUser);

			// check that user now appears in the list
			var users = api.Users.List();
			users.ShouldContain(u => u.Id == savedUser.Id);
			checkUsersAreEqual(user, savedUser);

			// update existing user
			user.DailyGoal += 100;
			user.IsAdmin = !user.IsAdmin;
			savedUser = api.Users.Save(user);
			checkUsersAreEqual(user, savedUser);

			api.Users.Delete(user.Id);
			api.Users.Delete(user.Id, HttpStatusCode.NotFound);
		}

		private static void checkUsersAreEqual(User expected, User actual)
		{
			actual.UserName.ShouldEqual(expected.UserName);
		}

		[TestMethod]
		public void CheckAdminRights()
		{
			var adminApi = new Api();
			adminApi.Account.Logon(new LogonModel { UserName = "admin", Password = "admin" });

			var userName = Guid.NewGuid().ToString();
			var password = "hello";
			var newUser = new User { UserName = userName, NewPassword = password };
			newUser = adminApi.Users.Save(newUser);
			newUser.Id.ShouldNotEqual(0);
			newUser.PasswordHash.ShouldBeNull();

			// by default new user does not have access rights
			var newUserApi = new Api();
			newUserApi.Account.Logon(new LogonModel { UserName = newUser.UserName, Password = newUser.NewPassword });
			newUserApi.Users.ListWithError(HttpStatusCode.Forbidden);

			// make him admin
			newUser.IsAdmin = true;
			adminApi.Users.Save(newUser);
			newUserApi.Users.List();

			// revoke admin rights
			newUser.IsAdmin = false;
			adminApi.Users.Save(newUser);
			newUserApi.Users.ListWithError(HttpStatusCode.Forbidden);
		}
	}
}
