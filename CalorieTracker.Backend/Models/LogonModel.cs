using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieTracker.Backend.Models
{
	/// <summary>
	/// Model for AuthenticationController.Logon method.
	/// </summary>
	public class LogonModel
	{
		/// <summary>
		/// User name.
		/// </summary>
		public string User { get; set; }

		/// <summary>
		/// User password.
		/// </summary>
		public string Password { get; set; }
	}
}
