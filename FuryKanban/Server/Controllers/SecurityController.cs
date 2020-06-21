using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuryKanban.Server.Contract;
using FuryKanban.Shared.Model.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FuryKanban.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SecurityController : ControllerBase
	{
		private ISecurityService _securityService;
		private ILogger<SecurityController> _logger;

		public SecurityController(ISecurityService securityService, ILogger<SecurityController> logger)
		{
			_securityService = securityService;
			_logger = logger;
		}

		[HttpPost("registration")]
		public async Task<RegistrationResponse> RegistrationAsync(RegistrationRequest registrationRequest)
		{
			return await _securityService.RegistrationAsync(registrationRequest);
		}
		
		// [HttpGet]
		// public RegistrationRequest Get()
		// {
		// 	return new RegistrationRequest() { 
		// 		Login = "123",
		// 		Password = "123"
		// 	};
		// 	//return await _securityService.RegistrationAsync(registration);
		// }
		//
		// [HttpPost("{login}")]
		// public async Task<bool> TryLogin(LoginRequest login)
		// {
		// 	return false;
		// }
	}
}
