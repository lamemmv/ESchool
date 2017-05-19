using System;
using System.Net;
using System.Text;
using ESchool.Services.Exceptions;
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
            var apiError = new ApiError(context.Exception);
            apiError.AssignErrorCodeAndMessage();

            WriteLog(context.HttpContext, apiError);

            var response = context.HttpContext.Response;
            response.StatusCode = (int)apiError.GetStatusCode();
            response.ContentType = "application/json";

            context.ExceptionHandled = true;
            context.Result = new JsonResult(apiError);
        }

        private void WriteLog(HttpContext httpContext, ApiError apiError)
        {
            if (apiError.GetStatusCode() == HttpStatusCode.InternalServerError)
            {
                var sb = new StringBuilder();

                try
                {
                    sb = LogHttpContext(httpContext)
                        .Append(apiError.GetExceptionDetail() ?? string.Empty);
                }
                catch (Exception)
                {
                    sb.Append(apiError.GetExceptionDetail() ?? string.Empty);
                }

                _logger.LogError(new EventId(0), sb.ToString()/*, exception*/);
            }
        }

        private StringBuilder LogHttpContext(HttpContext httpContext)
        {
            StringBuilder sb = new StringBuilder(256);
            var request = httpContext.Request;

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
