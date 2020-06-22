using FuryKanban.DataLayer;
using FuryKanban.Server.Contract;
using FuryKanban.Server.Logic;
using FuryKanban.Shared;
using FuryKanban.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuryKanban.Server.Filters
{
	public class AppStateFilter : Attribute, IActionFilter
	{
		private IAppStateService _appStateService;
		private AuthUser _authUser;

		public AppStateFilter(IAppStateService appStateService, AuthUser authUser)
		{
			_appStateService = appStateService;
			_authUser = authUser;
		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
			var action = String.Empty;
			if (context.HttpContext.Request.Headers.ContainsKey(Const.ActionTitle))
				action = context.HttpContext.Request.Headers[Const.ActionTitle].ToString();

			if (String.IsNullOrWhiteSpace(action))
				return;

			_appStateService.SetHistoryStateAsync(_authUser.Id, action).GetAwaiter().GetResult();
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			var error = ((ObjectResult)context.Result)?.Value as IErrorResult;
			if (!error.HasError)
			{
				_appStateService.SaveHistoryStateAsync();
			}

			var result = ((ObjectResult)context.Result)?.Value as IAppStateResult;
			if (result != null)
			{
				result.AppState = _appStateService.GetStateAsync(_authUser.Id).GetAwaiter().GetResult();
			}
		}
	}
}
