using FuryKanban.Logic;
using FuryKanban.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FuryKanban.Server.Filters
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TokenAuthorization : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext filterContext)
		{
			if (filterContext != null)
			{
				if (filterContext.HttpContext.Request.Headers.ContainsKey(Const.Token))
				{
					var token = filterContext.HttpContext.Request.Headers[Const.Token].ToString();

					var securityService = (ISecurityService)filterContext.HttpContext.RequestServices.GetService(typeof(ISecurityService));
					var userId = securityService.GetUserIdByTokenAsync(token).GetAwaiter().GetResult();

					if (userId.HasValue)
					{
						var authUser = (AuthUser)filterContext.HttpContext.RequestServices.GetService(typeof(AuthUser));
						authUser.Id = userId.Value;

						return;
					}
				}

				filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
				filterContext.Result = new JsonResult("NotAuthorized")
				{
					Value = new
					{
						Status = "Error",
						Message = "Invalid Token"
					},
				};
			}
		}
	}
}
