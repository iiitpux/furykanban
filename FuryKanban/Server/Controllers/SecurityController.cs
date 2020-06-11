using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuryKanban.Logic;
using FuryKanban.Shared.Model.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuryKanban.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SecurityController : ControllerBase
	{
		private ISecurityService _securityService;

		public SecurityController(ISecurityService securityService)
		{
			_securityService = securityService;
		}

		[HttpGet]
		public RegistrationModel Get()
		{
			return new RegistrationModel() { 
				Login = "123",
				Password = "123"
			};
			//return await _securityService.RegistrationAsync(registration);
		}

		[HttpPost("{login}")]
		public async Task<bool> TryLogin(LoginModel login)
		{
			return false;
		}
	}
}
