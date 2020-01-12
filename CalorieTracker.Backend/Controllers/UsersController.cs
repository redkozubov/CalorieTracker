using System.Threading.Tasks;
using System.Web.Http;
using CalorieTracker.Backend.Dal;
using CalorieTracker.Backend.Extensions;
using CalorieTracker.Backend.Models;

namespace CalorieTracker.Backend.Controllers
{
	/// <summary>
	/// Provides access to users.
	/// </summary>
	public class UsersController : ApiController
	{
		private UsersRepository getRepository()
		{
			return new UsersRepository(RequestContext.GetCurrentUserId());
		}

		/// <summary>
		/// Returns list of users.
		/// </summary>
		[HttpGet]
		public async Task<User[]> List()
		{
			return await getRepository().ListAsync();
		}

		/// <summary>
		/// Creates or updates user.
		/// </summary>
		[HttpPost]
		public async Task<User> Save(User user)
		{
			return await getRepository().SaveAsync(user);
		}

		/// <summary>
		/// Deletes user.
		/// </summary>
		[HttpDelete]
		public async Task Delete(int id)
		{
			await getRepository().DeleteAsync(id);
		}
	}
}
