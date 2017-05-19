using System;
using System.Net;
using System.Text;
using ESchool.Services.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ESchool.Services.Exceptions
{
    public sealed class ApiError
    {
        private readonly Exception _exception;

        public ApiError(Exception exception)
        {
            _exception = exception;
        }

        public ApiError(string type, string source, string message)
        {
            ErrorType = type;
            Source = source;
            ErrorMessage = message;
        }

        public string ErrorType { get; private set; }

        public int? ErrorCode { get; private set; }

        public string Source { get; }

        public string ErrorMessage { get; private set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore]
        public string ExceptionDetail { get; private set; }

        public void AssignErrorCodeAndMessage()
        {
            ErrorType = ApiErrorTypes.Business;
            ApiErrorCode errorCode;

            if (_exception is EntityDuplicateException)
            {
                errorCode = ApiErrorCode.DuplicateEntity;
                ErrorMessage = ((EntityDuplicateException)_exception).Message;
                StatusCode = HttpStatusCode.BadRequest;
            }
            else if (_exception is EntityNotFoundException)
            {
                errorCode = ApiErrorCode.NotFound;
                ErrorMessage = ((EntityNotFoundException)_exception).Message;
                StatusCode = HttpStatusCode.NotFound;
            }
            else if (_exception is UnauthorizedAccessException)
            {
                errorCode = ApiErrorCode.Unauthorized;
                ErrorMessage = "Unauthorized Access.";
                StatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                Exception innerException;

                if (_exception is DbUpdateException)
                {
                    ErrorType = ApiErrorTypes.Database;
                    innerException = _exception.InnerException;
                }
                else
                {
                    ErrorType = ApiErrorTypes.InternalServerError;
                    innerException = _exception;
                }

                errorCode = ApiErrorCode.InternalServerError;
#if !DEBUG
                message = "An unhandled error occurred.";                
#else
                ErrorMessage = innerException.Message;
#endif
                StatusCode = HttpStatusCode.InternalServerError;
                ExceptionDetail = GetExceptionDetail(innerException).ToString();
            }

            ErrorCode = (int)errorCode;
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
