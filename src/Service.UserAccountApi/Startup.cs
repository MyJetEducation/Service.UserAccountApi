﻿using System.Reflection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyJetWallet.Sdk.Service;
using Prometheus;
using Service.UserAccountApi.Modules;
using SimpleTrading.ServiceStatusReporterConnector;

namespace Service.UserAccountApi
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.BindCodeFirstGrpc();
			services.AddHostedService<ApplicationLifetimeManager>();
			services.AddMyTelemetry("ED-", Program.Settings.ZipkinUrl);
			services.SetupSwaggerDocumentation();
			services.ConfigurateHeaders();
			services.AddControllers();
			services.AddAuthentication(StartupUtils.ConfigureAuthenticationOptions)
				.AddJwtBearer(StartupUtils.ConfigureJwtBearerOptions);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseForwardedHeaders();
			app.UseRouting();
			app.UseStaticFiles();
			app.UseMetricServer();
			app.BindServicesTree(Assembly.GetExecutingAssembly());
			app.BindIsAlive();
			app.UseOpenApi();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseOpenApi(settings => settings.Path = "/api/v1/{documentName}/swagger/swagger.json");
			app.UseSwaggerUi3(settings =>
			{
				settings.Path = "/api/v1/{documentName}/swagger";
				settings.DocumentPath = "/api/v1/{documentName}/swagger/swagger.json";
			});

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapGet("/", async context => await context.Response.WriteAsync("MyJetEducation API endpoint"));
			});
		}

		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<SettingsModule>();
			builder.RegisterModule<ServiceModule>();
		}
	}
}