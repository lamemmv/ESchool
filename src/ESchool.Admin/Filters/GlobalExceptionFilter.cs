using System;
using System.Text;
using ESchool.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ESchool.Admin.Filters
{
    public sealed class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            ApiError apiError = new ApiError(exception);
            WriteLog(context.HttpContext, exception, apiError);

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)apiError.StatusCode;
            response.ContentType = "application/json";

            context.ExceptionHandled = true;
            context.Result = new JsonResult(apiError);
        }

        private void WriteLog(HttpContext httpContext, Exception exception, ApiError apiError)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb = LogHttpContext(httpContext)
                    .Append(apiError.ExceptionDetail ?? string.Empty);
            }
            catch (Exception)
            {
                sb.Append(apiError.ExceptionDetail ?? string.Empty);
            }

            _logger.LogError(new EventId(apiError.ErrorCode), exception, sb.ToString());
        }

        private StringBuilder LogHttpContext(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder(256);
            HttpRequest request = httpContext.Request;

            sb.Append("Url: ").Append(request.Path.Value).Append("\r\n");
            sb.Append("QueryString: ").Append(request.QueryString.ToString()).Append("\r\n");
            //sb.Append("Form: ").Append(request.Form.ToString()).Append("\r\n");
            sb.Append("Content Type: ").Append(request.ContentType).Append("\r\n");
            sb.Append("Content Length: ").Append(request.ContentLength).Append("\r\n");
            sb.Append("Remote IP: ").Append(httpContext.Connection.RemoteIpAddress.ToString());

            return sb;
        }
    }
}
