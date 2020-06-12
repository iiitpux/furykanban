using FuryKanban.Shared.Model.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FuryKanban.Logic
{
	public interface ISecurityService
	{
		Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registration);
	}
}
