using FuryKanban.Shared.Model.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuryKanban.Server.Contract
{
	public interface ISecurityService
	{
		Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registration);
		Task<LoginResponse> LoginAsync(LoginRequest login);
		Task<int?> GetUserIdByTokenAsync(string token);
	}
}
