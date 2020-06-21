using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using FuryKanban.DataLayer;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using FuryKanban.Server.Logic;
using FuryKanban.Server.Filters;
using FuryKanban.Server.Contract;

namespace FuryKanban.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			//todo- ��� ������� �� ������ ���������
			using (var client = new AppDbContext())
			{
				client.Database.EnsureCreated();
			}
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			//todo- add automapper
			services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();
			services.AddAuthorization();
			services.AddScoped<ISecurityService, SecurityService>();
			services.AddScoped<IAppStateService, AppStateService>();
			services.AddScoped<IIssueService, IssueService>();
			services.AddScoped<IStageService, StageService>();
			services.AddScoped<AuthUser>();
			services.AddScoped<AppStateFilter>();
			services.AddControllersWithViews();
			services.AddRazorPages();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				//app.UseHsts();
			}

			//
			//var cookiePolicyOptions = new CookiePolicyOptions
			//{
			//	MinimumSameSitePolicy = SameSiteMode.Strict,
			//};
			//app.UseCookiePolicy(cookiePolicyOptions);


			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
