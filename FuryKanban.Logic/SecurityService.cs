using FuryKanban.DataLayer;
using FuryKanban.Shared.Model.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Logic
{
	public class SecurityService : ISecurityService
	{
		private FkDbContext _fkDbContext;

		public SecurityService(FkDbContext fkDbContext)
		{
			_fkDbContext = fkDbContext;
		}

		public async Task<bool> RegistrationAsync(RegistrationModel registration)
		{
			var user = await _fkDbContext.Users.Where(p => p.Login == registration.Login).ToListAsync();
			throw new NotImplementedException();
		}
	}
}
