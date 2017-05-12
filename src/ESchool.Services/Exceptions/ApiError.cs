using System;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Services.Exceptions
{
    public sealed class ApiError
    {
        private string _exceptionDetail;
        private HttpStatusCode _httpStatusCode;

        private readonly Exception _exception;

        public ApiError(Exception exception)
        {
            _exception = exception;
        }

        public ApiError(ErrorCode code, string message)
        {
            Code = (int)code;
            Message = message;
        }

        public int Code { get; private set; }

        public string Message { get; private set; }

        public HttpStatusCode GetStatusCode()
        {
            return _httpStatusCode;
        }

        public string GetExceptionDetail()
        {
            return _exceptionDetail;
        }

        public void AssignErrorCodeAndMessage()
        {
            ErrorCode errorCode;
            string message;

            if (_exception is EntityDuplicateException)
            {
                errorCode = ErrorCode.DuplicateEntity;
                message = ((EntityDuplicateException)_exception).Message;
                _httpStatusCode = HttpStatusCode.BadRequest;
            }
            else if (_exception is EntityNotFoundException)
            {
                errorCode = ErrorCode.NotFound;
                message = ((EntityNotFoundException)_exception).Message;
                _httpStatusCode = HttpStatusCode.NotFound;
            }
            else if (_exception is UnauthorizedAccessException)
            {
                errorCode = ErrorCode.Unauthorized;
                message = "Unauthorized Access.";
                _httpStatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                var innerException = _exception is DbUpdateException ? _exception.InnerException : _exception;

                errorCode = ErrorCode.InternalServerError;
#if !DEBUG
                message = "An unhandled error occurred.";                
#else
                message = innerException.Message;
#endif
                _httpStatusCode = HttpStatusCode.InternalServerError;
                _exceptionDetail = GetExceptionDetail(innerException).ToString();
            }

            Code = (int)errorCode;
            Message = message;
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
