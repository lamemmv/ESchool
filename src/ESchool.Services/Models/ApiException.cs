using System;
using ESchool.Services.Enums;

namespace ESchool.Services.Models
{
    public sealed class ApiException : Exception
    {
        public ApiException(
            string message,
            ApiErrorCode errorCode = ApiErrorCode.InternalServerError)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public ApiException(
            Exception exception,
            ApiErrorCode errorCode = ApiErrorCode.InternalServerError)
            : base(exception.Message)
        {
            ErrorCode = errorCode;
        }

        public ApiErrorCode ErrorCode { get; }
    }
}
