﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuryKanban.Server.Contract;
using FuryKanban.Server.Filters;
using FuryKanban.Server.Logic;
using FuryKanban.Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuryKanban.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[TokenAuthorization]
	public class AppController : ControllerBase
	{
		private IAppStateService _appService;
		private AuthUser _authUser;
        public AppController(IAppStateService appService, AuthUser authUser)
		{
			_appService = appService;
			_authUser = authUser;
		}

        [HttpGet]
        public async Task<AppStateResponse> Get()
        {
			return new AppStateResponse()
			{
				AppState = await _appService.GetStateAsync(_authUser.Id)
			};
        }

		[HttpGet(template:"history/{id}")]
		public async Task<AppStateResponse> History(int id)
		{
			return await _appService.LoadHistory(id, _authUser.Id);
		}
    }
}
