using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CalorieTracker.Backend.Controllers.Models;
using CalorieTracker.Backend.Exceptions;
using CalorieTracker.Backend.Extensions;
using CalorieTracker.Backend.Models;

namespace CalorieTracker.Backend.Dal
{
	/// <summary>
	/// Provides administrator access to users.
	/// </summary>
	class UsersRepository
	{
		private readonly CalorieTrackerContext _context;
		private readonly int? _userId;

		public UsersRepository(int? userId)
		{
			_context = new CalorieTrackerContext();
			_userId = userId;
			var currentUser = _context.Users.SingleOrDefault(u => u.Id == userId);
			if (currentUser == null || !currentUser.IsAdmin)
			{
				// current user was deleted from the database or does not have sufficient permissions
				throw new UnauthorizedAccessException();
			}
		}

		/// <summary>
		/// Returns list of users.
		/// </summary>
		public async Task<User[]> ListAsync()
		{
			return await _context.Users.ToArrayAsync();
		}

		/// <summary>
		/// Finds user by name.
		/// </summary>
		public async Task<User> FindAsync(string userName, int? id = null)
		{
			var users = _context.Users.AsQueryable();
			if (id.HasValue)
			{
				users = users.Where(u => u.Id == id);
			}
			return await users.SingleOrDefaultAsync(u => u.UserName == userName);
		}

		/// <summary>
		/// Creates new or updates existing user with permissions check.
		/// </summary>
		public async Task<User> SaveAsync(User user)
		{
			if (user.Id != 0)
			{
				var dbUser = await _context.Users.SingleOrNotFoundAsync(u => u.Id == user.Id, "User not found");
				if (user.UserName != "admin")
				{
					dbUser.IsAdmin = user.IsAdmin;
				}
				dbUser.DailyGoal = user.DailyGoal;
				dbUser.NewPassword = user.NewPassword;
				user = dbUser;
			}
			else
			{
				if (await _context.Users.AnyAsync(u => u.UserName == user.UserName))
				{
					throw new BadRequestException("User already exists");
				}
				_context.Users.Add(user);
			}
			if (!string.IsNullOrEmpty(user.NewPassword))
			{
				user.PasswordHash = AccountRepository.HashPassword(user.NewPassword);
			}
			await _context.SaveChangesAsync();
			return user;
		}

		/// <summary>
		/// Deletes user.
		/// </summary>
		public async Task DeleteAsync(int userId)
		{
			var user = await _context.Users.SingleOrNotFoundAsync(u => u.Id == userId, "User not found");
			if (user.UserName == "admin")
			{
				throw new BadRequestException("Cannot delete admin");
			}
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
		}
	}
}
