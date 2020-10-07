using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateService.Api
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private readonly ILogger<ExceptionHandlingMiddleware> logger;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			this.next = next;
			this.logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}

		private Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			logger.LogError(exception, "Request resulted with an error");

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.Conflict;

			var serializedResult = JsonConvert.SerializeObject(exception.Message);
			return context.Response.WriteAsync(serializedResult);
		}
	}
}
