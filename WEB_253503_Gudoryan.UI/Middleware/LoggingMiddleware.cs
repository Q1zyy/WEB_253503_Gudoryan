using Serilog;
using System.Diagnostics;

namespace WEB_253503_Gudoryan.UI.Middleware
{
	public class LoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public LoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			await _next(context);

			var statusCode = context.Response.StatusCode;
			var requestPath = context.Request.Path;

		
			if (statusCode < 200 || statusCode >= 300)
			{
				var logMessage = $"---> request {requestPath} returns {statusCode}";
				Log.Information(logMessage);
			}

		}
	}
}
