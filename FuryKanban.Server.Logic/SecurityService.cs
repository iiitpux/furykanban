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
using FuryKanban.Server.Contract;

namespace FuryKanban.Server.Logic
{
	public class SecurityService : ISecurityService
	{
		private AppDbContext _appDbContext;
		private ILogger<SecurityService> _logger;

		public SecurityService(AppDbContext appDbContext, ILogger<SecurityService> logger)
		{
			_appDbContext = appDbContext;
			_logger = logger;
		}

		public async Task<int?> GetUserIdByTokenAsync(string token)
		{
			var existToken = await _appDbContext.Tokens.SingleOrDefaultAsync(p => p.Code == token);
			if (existToken != null)
			{
				return existToken.UserId;
			}
			return null;
		}

		public async Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registration)
		{
			var user = await _appDbContext.Users.SingleOrDefaultAsync(p => p.Login == registration.Login);
			if (user != null)
				return new RegistrationResponse() { HasError = true, ErrorMessage = "User with same login already exist" };

			string salt = Guid.NewGuid().ToString();
			var newUser = new UserDto()
			{
				Active = true,
				Login = registration.Login,
				Password = Hashing.GetPasswordHash(registration.Password, salt),
				Salt = salt,
				CreateDate = DateTime.Now
			};

			var token = new TokenDto()
			{
				Code = Guid.NewGuid().ToString(),
				CreatedDate = DateTime.Now
			};

			newUser.Tokens.Add(token);

			var isValid = ValidationChecker.Check<UserDto>(newUser, out var results);
			if (!isValid)
			{
				_logger.Log(LogLevel.Error, "Model is not valid because " + string.Join(", ", results.Select(s => s.ErrorMessage).ToArray()));
				return new RegistrationResponse() { HasError = true, ErrorMessage = "Server error" };
			}

			await _appDbContext.AddAsync<UserDto>(newUser);
			await _appDbContext.SaveChangesAsync();

			return new RegistrationResponse()
			{
				Token = token.Code
			};
		}

		//public async Task<LoginResponse> LoginAsync(LoginRequest login)
		//{

		//}
	}
}
