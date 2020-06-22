using FuryKanban.DataLayer;
using FuryKanban.Server.Contract;
using FuryKanban.Server.Logic;
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
			//todo change to header actionName
			var method = context.HttpContext.Request.Method;
			var action = String.Empty;
			switch (method)
			{
				case "POST":
					{
						action = "Insert";
						break;
					}
				case "PUT":
					{
						action = "Update";
						break;
					}
				case "DELETE":
					{
						action = "Delete";
						break;
					}
			}
			if (String.IsNullOrWhiteSpace(action))
				return;

			_appStateService.SetHistoryStateAsync(_authUser.Id, action);
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			var result = ((ObjectResult)context.Result)?.Value as IAppStateResult;
			if (result != null)
			{
				result.AppState = _appStateService.GetStateAsync(_authUser.Id).GetAwaiter().GetResult();
			}

			var error = ((ObjectResult)context.Result).Value as IErrorResult;
			if(!error.HasError)
			{
				_appStateService.SaveHistoryStateAsync();
			}
		}
	}
}
