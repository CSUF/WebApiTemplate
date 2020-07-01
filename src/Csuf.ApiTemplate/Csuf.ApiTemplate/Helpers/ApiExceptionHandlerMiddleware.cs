// (c) California State University, Fullerton.  All rights reserved.

namespace Csuf.ApiTemplate
{
	using Microsoft.AspNetCore.Diagnostics;
	using Microsoft.AspNetCore.Http;
	using Newtonsoft.Json;
	using Serilog;
	using System;
	using System.Threading.Tasks;

	public class ApiExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public ApiExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next.Invoke(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			//var logger = loggerFactory.CreateLogger("Serilog Global exception logger");
			Log.Fatal(exception: exception, messageTemplate: exception.Message);
			IExceptionHandlerFeature exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
			if (exceptionHandlerFeature != null && exceptionHandlerFeature.Error != null)
			{
				//logger.LogError(eventId: StatusCodes.Status500InternalServerError, exception: exceptionHandlerFeature.Error, message: exceptionHandlerFeature.Error.Message);
				Log.Fatal(exception: exceptionHandlerFeature.Error, messageTemplate: exceptionHandlerFeature.Error.Message);
			}
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsync(JsonConvert.SerializeObject(new
			{
				id = context.TraceIdentifier,
				error = $"An error occurred in our API.  Please refer to the Error ID with the support. Technical information: {exception.GetType().Name}: {exception.InnerException?.Message} | {exception.Message} | {exception.Source}"
			}));
		}
	}
}