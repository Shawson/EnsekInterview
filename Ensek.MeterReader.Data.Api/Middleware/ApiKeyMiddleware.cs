using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ensek.MeterReading.Data.Api.Middleware
{
	public class ApiKeyMiddleware
	{
		private readonly RequestDelegate _next;
		private const string APIKEYNAME = "ApiKey";
		private const string APIKEYHEADER = "X-MeterReadingData-ApiKey";
		public ApiKeyMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		public async Task InvokeAsync(HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue(APIKEYHEADER, out var extractedApiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync($"Missing {APIKEYHEADER} HTTP Header");

				return;
			}

			var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

			var apiKey = appSettings.GetValue<string>(APIKEYNAME);

			if (!apiKey.Equals(extractedApiKey))
			{
				context.Response.StatusCode = 401;
				await context.Response.WriteAsync($"Invalid {APIKEYHEADER} HTTP Header value");
				return;
			}

			await _next(context);
		}
	}
}
