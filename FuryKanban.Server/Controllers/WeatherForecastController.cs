using FuryKanban.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FuryKanban.Server.Filters;

namespace FuryKanban.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[TokenAuthorization]
	public class WeatherForecastController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<WeatherForecastController> logger;

		public WeatherForecastController(ILogger<WeatherForecastController> logger)
		{
			this.logger = logger;
		}

		[HttpGet]
		public bool Get()
		{
			//Set("keyco", "test", null);
			return true;
		}
		//public void Set(string key, string value, int? expireTime)
		//{
		//	CookieOptions option = new CookieOptions();

		//	if (expireTime.HasValue)
		//		option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
		//	else
		//		option.Expires = DateTime.Now.AddDays(10);

		//	Response.Cookies.Append(key, value, option);
		//}
	}
}
