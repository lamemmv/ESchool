using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ESchool.Services.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ESchool.Services.Models
{
    public sealed class ApiError
    {
        public ApiError(Exception exception)
        {
            AssignErrorCodeAndMessage(exception);
        }

        public ApiError(ModelStateDictionary modelState)
        {
            ErrorCode = (int)ApiErrorCode.ValidateViewModelFail;
            ValidationErrors = from kvp in modelState
                               from e in kvp.Value.Errors
                               let k = kvp.Key
                               select new ValidationError(k, e.ErrorMessage);
        }

        public int ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        public IEnumerable<ValidationError> ValidationErrors { get; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore]
        public string ExceptionDetail { get; private set; }

        private void AssignErrorCodeAndMessage(Exception exception)
        {
            Type exceptionType = exception.GetType();
            ApiErrorCode errorCode = GetApiErrorCode(exception, exceptionType);

            if (errorCode != ApiErrorCode.Undefined)
            {
                ErrorMessage = exception.Message;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                errorCode = ApiErrorCode.Unauthorized;
                ErrorMessage = "Unauthorized Access.";
                StatusCode = HttpStatusCode.Unauthorized;
            }
            else
            {
                Exception innerException = exceptionType == typeof(DbUpdateException) ?
                    exception.InnerException :
                    exception;

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

        private ApiErrorCode GetApiErrorCode(Exception exception, Type exceptionType)
        {
            ApiErrorCode errorCode = ApiErrorCode.Undefined;

            if (exceptionType == typeof(DuplicateQTagException))
            {
                errorCode = ApiErrorCode.DuplicateEntity;
                StatusCode = HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(EntityNotFoundException))
            {
                errorCode = ApiErrorCode.NotFound;
                StatusCode = HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(RandomExamPaperException))
            {
                errorCode = ApiErrorCode.RandomExamPaperError;
                StatusCode = HttpStatusCode.BadRequest;
            }

            return errorCode;
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
