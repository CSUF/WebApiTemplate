// (c) California State University, Fullerton.  All rights reserved.

using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Csuf.ApiTemplate
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

			string devEnvironmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			bool isDevelopment = string.IsNullOrWhiteSpace(devEnvironmentVariable) || devEnvironmentVariable.Equals("Development", StringComparison.OrdinalIgnoreCase);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddIdentityServerAuthentication(options =>
			{
				options.Authority = Configuration["CsufApiTemplate:IdentityServerAuthorityUrl"];
				options.RequireHttpsMetadata = false;
				options.LegacyAudienceValidation = true;
				options.ApiName = "sampleApi";
				options.EnableCaching = true;
				if (!int.TryParse(Configuration["CsufApiTemplate:IdentityServerIntrospectionCacheMinutes"], out int cachePeriod)) cachePeriod = 20;
				options.CacheDuration = TimeSpan.FromMinutes(cachePeriod);
				options.JwtBearerEvents = new JwtBearerEvents()
				{
					OnAuthenticationFailed = (context) =>
					{
						Log.Debug("OnAuthenticationFailed");
						return Task.FromResult(0);
					},
					OnChallenge = (context) =>
					{
						Log.Debug("OnChallenge");
						return Task.FromResult(0);
					},
					OnMessageReceived = (context) =>
					{
						Log.Debug("OnMessageReceived");
						return Task.FromResult(0);
					},
					OnTokenValidated = (context) =>
					{
						Log.Debug("OnTokenValidated");
						return Task.FromResult(0);
					},
				};
			}
			);

			services.AddControllers(setupAction => setupAction.ReturnHttpNotAcceptable = true)
							.AddNewtonsoftJson(setupAction =>
							{
								setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
								setupAction.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
								setupAction.SerializerSettings.Formatting = Formatting.Indented;
								setupAction.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
							});

			services.AddRazorPages();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.Use(async (context, next) =>
			{
				if (context.Request.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase)) context.Request.Scheme = "https";
				await next.Invoke();
			});

			app.UseStatusCodePagesWithReExecute(pathFormat: "/error", queryFormat: "?statusCode={0}");
			app.UseMiddleware<ApiExceptionHandlerMiddleware>();
			app.UseHsts();

			app.UseHttpsRedirection();

			app.UseSerilogRequestLogging(opts =>
			{
				opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
				opts.GetLevel = LogHelper.ExcludeHealthChecks; // Use the custom level
			});
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers().RequireAuthorization();
				endpoints.MapControllerRoute(name: "DefaultRoute", pattern: "{controller=Default}/{action=Index}/{id?}").RequireAuthorization();
			});
		}
	}
}