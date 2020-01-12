using System;
using System.Net;
using System.Net.Http;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Tests.Wrappers;
using Microsoft.Owin.Testing;

namespace CalorieTracker.Backend.Tests
{
	/// <summary>
	/// Provides access to the API.
	/// </summary>
	class Api
	{
		private readonly HttpClient _client;
		private readonly TestServer _server;

		public Api()
		{
			_server = TestServer.Create(new Startup(true).Configuration);
			var handler = new CookieHandler()
			{
				CookieContainer = new CookieContainer(),
				InnerHandler = _server.Handler
			};
            _client = new HttpClient(handler)
				{
					BaseAddress = _server.BaseAddress
				};
			Account = new Account(_client);
			Entries = new Entries(_client);
			Users = new Users(_client);
			Statistics = new Statistics(_client);
		}

		/// <summary>
		/// Account API.
		/// </summary>
		public Account Account { get; private set; }

		/// <summary>
		/// Entries API.
		/// </summary>
		public Entries Entries { get; private set; }

		/// <summary>
		/// Users API.
		/// </summary>
		public Users Users { get; set; }

		/// <summary>
		/// Statistics API.
		/// </summary>
		public Statistics Statistics { get; set; }

		/// <summary>
		/// Registers a new user, authorizes and returns instance of API.
		/// </summary>
		/// <returns></returns>
		public static Api RegisterAndLogon()
		{
			var user = new LogonModel { UserName = Guid.NewGuid().ToString(), Password = "some password" };
			var api = new Api();
			api.Account.Register(user);
			api.Account.Logon(user);
			return api;
		}
	}
}
