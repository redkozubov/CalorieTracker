using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CalorieTracker.Backend.Models
{
	/// <summary>
	/// Represents a user.
	/// </summary>
	public class User
	{
		/// <summary>
		/// Unique identifier.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// User name, aka login.
		/// </summary>
		[Index(IsUnique = true)]
		[MaxLength(500)]
		public string UserName { get; set; }

		/// <summary>
		/// User is admin.
		/// </summary>
		public bool IsAdmin { get; set; }

		/// <summary>
		/// Daily goal for calories intake.
		/// </summary>
		public int? DailyGoal { get; set; }

		/// <summary>
		/// Password hash.
		/// </summary>
		[JsonIgnore]
		public string PasswordHash { get; set; }

		/// <summary>
		/// Set on front-end in case if password needs to be changed.
		/// </summary>
		[NotMapped]
		public string NewPassword { get; set; }
	}
}
