using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Dal;
using CalorieTracker.Backend.Exceptions;
using CalorieTracker.Backend.Extensions;
using CalorieTracker.Backend.Filters;
using CalorieTracker.Backend.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CalorieTracker.Backend.Controllers
{
	/// <summary>
	/// Manages logon/logoff and user registration.
	/// </summary>
	[RoutePrefix("api/account")]
	public class AccountController : ApiController
	{
		private AccountRepository getRepository()
		{
			return new AccountRepository(RequestContext.GetCurrentUserId());
		}

		/// <summary>
		/// Registers a new user.
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[Route("register")]
		public async Task<User> Register(LogonModel logon)
		{
			return await getRepository().Register(logon);
		}

		/// <summary>
		/// Authenticates user.
		/// </summary>
		[HttpPost]
		[AllowAnonymous]
		[Route("logon")]
		public async Task<User> Logon(LogonModel logon)
		{
			var user = await getRepository().ValidateCredentials(logon);

			var claims = new List<Claim>();
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Name, user.UserName));
			var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
			Request.GetOwinContext().Authentication.SignIn(
				new AuthenticationProperties() { IsPersistent = true },
				identity);
			return user;
		}

		/// <summary>
		/// Returns info about current user.
		/// </summary>
		[HttpGet]
		[Route("current")]
		public async Task<User> GetCurrent()
		{
			return await getRepository().GetCurrentUserAsync();
		}

		/// <summary>
		/// Updates current user.
		/// </summary>
		/// <remarks>Only updates allowed values: password and calories intake.</remarks>
		[HttpPut]
		[Route("current")]
		public async Task<User> SetCurrent(User user)
		{
			return await getRepository().SaveCurrentUserAsync(user);
		}

		/// <summary>
		/// Deauthenticates user.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("logoff")]
		public void Logoff()
		{
			Request.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
		}
	}
}
