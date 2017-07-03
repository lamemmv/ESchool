using System;
using System.Net;
using System.Text;
using ESchool.Services.Enums;
using ESchool.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ESchool.Admin.Filters
{
    public sealed class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        private ApiErrorCode _errorCode = ApiErrorCode.InternalServerError;
        private string _errorMessage = null;
        private HttpStatusCode _statusCode = HttpStatusCode.InternalServerError;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            AssignGlobalParameters(exception);

            WriteLog(context.HttpContext, exception, (int)_statusCode);

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)_statusCode;
            response.ContentType = "application/json";

            context.ExceptionHandled = true;
            context.Result = new JsonResult(new ApiError(_errorCode, _errorMessage));
        }

        private void AssignGlobalParameters(Exception exception)
        {
            Type exceptionType = exception.GetType();

            if (exceptionType == typeof(ApiException))
            {
                ApiException apiException = (ApiException)exception;
                _errorCode = apiException.ErrorCode;
                _errorMessage = apiException.Message;

                if (_errorCode == ApiErrorCode.NotFound)
                {
                    _statusCode = HttpStatusCode.NotFound;
                }
                else if (_errorCode == ApiErrorCode.DuplicateEntity ||
                    _errorCode == ApiErrorCode.RandomExamPaperError)
                {
                    _statusCode = HttpStatusCode.BadRequest;
                }
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                _errorCode = ApiErrorCode.Unauthorized;
                _errorMessage = "Unauthorized Access.";
                _statusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                Exception innerException = exceptionType == typeof(DbUpdateException) ?
                    exception.InnerException :
                    exception;

                _errorCode = ApiErrorCode.InternalServerError;
#if !DEBUG
                _errorMessage = "An unhandled error occurred.";                
#else
                _errorMessage = innerException.Message;
#endif
            }
        }

        private void WriteLog(HttpContext httpContext, Exception exception, int eventId)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                sb = LogHttpContext(httpContext)
                    .Append(GetExceptionDetail(exception));
            }
            catch (Exception)
            {
            }

            _logger.LogError(new EventId(eventId), exception, sb.ToString());
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

        private StringBuilder GetExceptionDetail(Exception exception)
        {
            StringBuilder sb = new StringBuilder(256);

            if (exception != null)
            {
                sb.Append("Source: ").Append(exception.Source).Append("\r\n");
                sb.Append("Message: ").Append(exception.Message).Append("\r\n");
                sb.Append("Detail: ").Append(exception.ToString());
            }

            return sb;
        }
    }
}
