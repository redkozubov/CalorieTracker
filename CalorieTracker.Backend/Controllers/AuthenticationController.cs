using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CalorieTracker.Backend.Filters;
using CalorieTracker.Backend.Models;

namespace CalorieTracker.Backend.Controllers
{
	[ErrorHandling]
	public class AuthenticationController : ApiController
	{
		public AuthenticationController()
		{
			
		}

		/// <summary>
		/// Performs user authentication.
		/// </summary>
		[AllowAnonymous]
		public void Logon(LogonModel model)
		{

		}
	}
}
