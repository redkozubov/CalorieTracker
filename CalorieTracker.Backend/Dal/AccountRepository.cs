using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Exceptions;
using CalorieTracker.Backend.Extensions;
using CalorieTracker.Backend.Models;
using Microsoft.AspNet.Identity;

namespace CalorieTracker.Backend.Dal
{
	/// <summary>
	/// Represents User logic available for anonymous or current user.
	/// </summary>
	class AccountRepository
	{
		private readonly CalorieTrackerContext _context;
		private readonly int? _userId;

		public AccountRepository(int? userId)
		{
			_context = new CalorieTrackerContext();
			_userId = userId;
		}

		public static string HashPassword(string password)
		{
			return new PasswordHasher().HashPassword(password);
        }

		/// <summary>
		/// Registers a new user.
		/// </summary>
		public async Task<User> Register(LogonModel logon)
		{
			if (await _context.Users.AnyAsync(u => u.UserName == logon.UserName))
			{
				throw new BadRequestException("User already exists");
			}
			var passwordHash = HashPassword(logon.Password);
			var user = new User
			{
				UserName = logon.UserName,
				PasswordHash = passwordHash,
				IsAdmin = false,
			};
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}

		/// <summary>
		/// Validates provided credentials.
		/// </summary>
		public async Task<User> ValidateCredentials(LogonModel logon)
		{
			var user = await _context.Users.SingleOrNotFoundAsync(u => u.UserName == logon.UserName, "User not found");
			var verificationResult = new PasswordHasher().VerifyHashedPassword(user.PasswordHash, logon.Password);
			if (verificationResult == PasswordVerificationResult.Failed)
			{
				throw new BadRequestException("Incorrect password");
			}
			return user;
		}

		/// <summary>
		/// Returns current user from database.
		/// </summary>
		public async Task<User> GetCurrentUserAsync()
		{
			var user = await _context.Users.SingleOrNotFoundAsync(u => u.Id == _userId, "User not found");
			if (user == null)
			{
				throw new UnauthorizedAccessException();
			}
			return user;
		}

		/// <summary>
		/// Updates current user parametes, only allowed values.
		/// </summary>
		public async Task<User> SaveCurrentUserAsync(User user)
		{
			var userInDb = await GetCurrentUserAsync();
			userInDb.DailyGoal = user.DailyGoal;
			if (!string.IsNullOrEmpty(user.NewPassword))
			{
				userInDb.PasswordHash = HashPassword(user.NewPassword);
			}
			await _context.SaveChangesAsync();
			return userInDb;
		}
	}
}
