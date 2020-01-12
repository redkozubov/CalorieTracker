using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CalorieTracker.Backend.Models;
using CalorieTracker.Backend.Tests.Extensions;

namespace CalorieTracker.Backend.Tests.Wrappers
{
	/// <summary>
	/// Users API.
	/// </summary>
	class Users
	{
		private readonly HttpClient _client;

		public Users(HttpClient client)
		{
			_client = client;
		}

		/// <summary>
		/// Returns list of users.
		/// </summary>
		public User[] List()
		{
			return _client.GetAsync("/api/users").ReadResponse<User[]>();
		}

		/// <summary>
		/// Requests list of users and expects an error.
		/// </summary>
		public void ListWithError(HttpStatusCode expectedCode)
		{
			_client.GetAsync("/api/users").ReadResponse(expectedCode);
		}

		/// <summary>
		/// Creates new or updates existing user.
		/// </summary>
		public User Save(User user)
		{
			return _client.PostAsJsonAsync("/api/users", user).ReadResponse<User>();
		}

		/// <summary>
		/// Deletes user.
		/// </summary>
		public void Delete(int id, HttpStatusCode expectedCode = HttpStatusCode.NoContent)
		{
			_client.DeleteAsync("/api/users/" + id).ReadResponse(expectedCode);
		}
	}
}
