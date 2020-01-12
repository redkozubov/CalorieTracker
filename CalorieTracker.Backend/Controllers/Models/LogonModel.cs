namespace CalorieTracker.Backend.Controllers.Models
{
	/// <summary>
	/// Model for user logon action.
	/// </summary>
	public class LogonModel
	{
		/// <summary>
		/// User name.
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Password.
		/// </summary>
		public string Password { get; set; }
	}
}
