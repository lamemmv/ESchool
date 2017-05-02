using System;
using System.Text;
using ESchool.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            Exception exception = context.Exception;

            ApiError apiError = HandleApiException(exception);

            if (apiError == null)
            {
                if (exception is UnauthorizedAccessException)
                {
                    apiError = new ApiError(ErrorCode.Unauthorized, "Unauthorized Access.");
                }
                else if (exception is DbUpdateException)
                {
                    var innerException = exception.InnerException;

                    apiError = new ApiError(ErrorCode.InternalServerError, innerException.Message, innerException.ToString());
                }
                else
                {
#if !DEBUG
                    string message = "An unhandled error occurred.";                
                    string stackTrace = null;
#else
                    string message = exception.Message;
                    string stackTrace = exception.ToString();
#endif

                    apiError = new ApiError(ErrorCode.InternalServerError, message, stackTrace);
                }
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
                //sb.Append(message);
            }

            _logger.LogError(new EventId(0), sb.ToString(), exception);
            // End write log.

            var response = context.HttpContext.Response;
            response.StatusCode = apiError.StatusCode;
            response.ContentType = "application/json";

            context.ExceptionHandled = true;

            //response.WriteAsync($"{message} {exception.StackTrace}");
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }

        private ApiError HandleApiException(Exception exception)
        {
            ApiError apiError = null;

            if (exception is EntityDuplicateException)
            {
                apiError = new ApiError(ErrorCode.DuplicateEntity, ((EntityDuplicateException)exception).Message);
            }
            else if (exception is EntityNotFoundException)
            {
                apiError = new ApiError(ErrorCode.DuplicateEntity, ((EntityNotFoundException)exception).Message);
            }

            return apiError;
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
