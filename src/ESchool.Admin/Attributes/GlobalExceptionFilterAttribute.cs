using System;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESchool.Admin.Attributes
{
    public sealed class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            HttpStatusCode status;
            string message = string.Empty;

            var exception = context.Exception;
            var exceptionType = exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(DbUpdateException))
            {
                exception = exception.InnerException;

                message = exception.ToString();
                status = HttpStatusCode.InternalServerError;
            }
            else if (exceptionType == typeof(Exception))
            {
                message = exception.ToString();
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                message = exception.Message;
                status = HttpStatusCode.NotFound;
            }

            // Write log.
            var sb = new StringBuilder();

            try
            {
                sb = LogHttpContext(context.HttpContext)
                    .Append(LogException(exception));
            }
            catch (Exception)
            {
                sb.Append(message);
            }

            _logger.LogError(new EventId(0), sb.ToString(), exception);
            // End write log.

            var response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            //response.ContentType = "application/json";
            //response.Headers.Add("Access-Control-Allow-Origin", "*");

            context.ExceptionHandled = true;

            response.WriteAsync($"{message} {exception.StackTrace}");
        }

        private StringBuilder LogHttpContext(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder(256);
            var request = httpContext.Request;

            sb.Append("URL: ").Append(request.Path.Value).Append("\r\n");
            sb.Append("QUERYSTRING: ").Append(request.QueryString.ToString()).Append("\r\n");
            //sb.Append("FORM: ").Append(request.Form.ToString()).Append("\r\n");
            sb.Append("REMOTE IP: ").Append(httpContext.Connection.RemoteIpAddress);

            return sb;
        }

        private StringBuilder LogException(Exception exception)
        {
            StringBuilder sb = new StringBuilder(256);

            if (exception != null)
            {
                sb.Append("SOURCE: ").Append(exception.Source).Append("\r\n");
                sb.Append("MESSAGE: ").Append(exception.Message).Append("\r\n");
                sb.Append("DETAIL: ").Append(exception.ToString());
            }

            return sb;
        }
    }
}
