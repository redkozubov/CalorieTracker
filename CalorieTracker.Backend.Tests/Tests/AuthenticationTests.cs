using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Backend.Controllers.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace CalorieTracker.Backend.Tests.Tests
{
	[TestClass]
	public class AuthenticationTests
	{
		/// <summary>
		/// Registers new user, does logon and logoff.
		/// </summary>
		[TestMethod]
		public void LogonLogoff()
		{
			var api = new Api();

			var userName = Guid.NewGuid().ToString();
			var password = "correct password";
			api.Account.Register(new LogonModel { UserName = userName, Password = password });
			api.Account.Logon(new LogonModel { UserName = userName, Password = "wrong password" }, HttpStatusCode.BadRequest);

			var user = api.Account.Logon(new LogonModel { UserName = userName, Password = password });
			user.IsAdmin.ShouldBeFalse();
			user.UserName.ShouldEqual(userName);
			user.PasswordHash.ShouldBeNull();

			api.Account.Logoff();
			api.Account.Logoff(HttpStatusCode.Unauthorized);
		}

		/// <summary>
		/// Ensures user can't CRUD without logon.
		/// </summary>
		[TestMethod]
		public void Authorization()
		{
			var api = new Api();
			var entry = EntriesTests.CreateEntry();
			api.Entries.SaveWithError(entry, HttpStatusCode.Unauthorized);
			api.Entries.ListWithError(HttpStatusCode.Unauthorized);
		}
	}
}
