using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Models;
using CalorieTracker.Backend.Tests.Extensions;

namespace CalorieTracker.Backend.Tests.Wrappers
{
	/// <summary>
	/// Represents a wrapper around account API.
	/// </summary>
	class Account
	{
		private readonly HttpClient _client;

		public Account(HttpClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Registers a new user.
		/// </summary>
		public User Register(LogonModel model)
		{
			return _client.PostAsJsonAsync("/api/account/register", model).ReadResponse<User>();
		}

		/// <summary>
		/// Performs user logon.
		/// </summary>
		public User Logon(LogonModel model, HttpStatusCode expectedCode = HttpStatusCode.OK)
		{
			return _client.PostAsJsonAsync("/api/account/logon", model).ReadResponse<User>(expectedCode);
		}

		/// <summary>
		/// Performs user logoff.
		/// </summary>
		public void Logoff(HttpStatusCode expectedCode = HttpStatusCode.NoContent)
		{
			_client.PostAsJsonAsync<object>("/api/account/logoff", null).ReadResponse(expectedCode);
		}
	}
}
