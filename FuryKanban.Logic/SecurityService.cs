using FuryKanban.DataLayer;
using FuryKanban.Shared.Model.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FuryKanban.Common;
using FuryKanban.DataLayer.Dto;
using Microsoft.Extensions.Logging;

namespace FuryKanban.Logic
{
	public class SecurityService : ISecurityService
	{
		private FkDbContext _fkDbContext;
		private ILogger<SecurityService> _logger;

		public SecurityService(FkDbContext fkDbContext, ILogger<SecurityService> logger)
		{
			_fkDbContext = fkDbContext;
			_logger = logger;
		}

		public async Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registration)
		{
			var user = await _fkDbContext.Users.SingleOrDefaultAsync(p => p.Login == registration.Login);
			if(user != null)
				return new RegistrationResponse(){HasError = true, ErrorMessage = "User with same login already exist"};
			
			string salt = Guid.NewGuid().ToString();
			var newUser = new UserDto()
			{
				Active = true,
				Login = registration.Login,
				Password = Hashing.GetPasswordHash(registration.Password, salt),
				Salt = salt,
				CreateDate = DateTime.Now
			};

			var isValid = ValidationChecker.Check<UserDto>(newUser, out var results);
			if (!isValid)
			{
				_logger.Log(LogLevel.Error, "Model is not valid because " + string.Join(", ", results.Select( s => s.ErrorMessage).ToArray()));
				return new RegistrationResponse(){HasError = true, ErrorMessage = "Server error"};
			}
				
			await _fkDbContext.AddAsync<UserDto>(newUser);
			await _fkDbContext.SaveChangesAsync();

			return new RegistrationResponse();
		}

		//public async Task<LoginResponse> LoginAsync(LoginRequest login)
		//{

		//}
	}
}
