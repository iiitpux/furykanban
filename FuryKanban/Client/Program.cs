using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using FuryKanban.Client.Core;
using FuryKanban.Client.Core.Security;
using Blazored.LocalStorage;
using Blazor.DragDrop.Core;

namespace FuryKanban.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("app");

			builder.Services.AddBlazoredLocalStorage();
			builder.Services.AddScoped(provider => new AppHttpClient(provider.GetRequiredService<ILocalStorageService>()) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
			builder.Services.AddScoped<StageService>();
			builder.Services.AddScoped<IssueService>();
			builder.Services.AddScoped<AppStateService>();
			//todo dragndrop
			//builder.Services.AddBlazorDragDrop();
			
			#region auth
			builder.Services.AddScoped<SecurityService>();
			builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<SecurityService>());
			builder.Services.AddAuthorizationCore();
			#endregion

			await builder.Build().RunAsync();
		}
	}
}
