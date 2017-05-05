﻿using System;
using System.Net;
using System.Text;
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

        public ApiError(ErrorCode code, string message)
        {
            Code = (int)code;
            Message = message;
        }

        public int Code { get; private set; }

        public string Message { get; private set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; private set; }

        [JsonIgnore]
        public string ExceptionDetail { get; private set; }

        public void AssignErrorCodeAndMessage()
        {
            ErrorCode errorCode;
            string message;

            if (_exception is EntityDuplicateException)
            {
                errorCode = ErrorCode.DuplicateEntity;
                message = ((EntityDuplicateException)_exception).Message;
                StatusCode = HttpStatusCode.BadRequest;
            }
            else if (_exception is EntityNotFoundException)
            {
                errorCode = ErrorCode.NotFound;
                message = ((EntityNotFoundException)_exception).Message;
                StatusCode = HttpStatusCode.NotFound;
            }
            else if (_exception is UnauthorizedAccessException)
            {
                errorCode = ErrorCode.Unauthorized;
                message = "Unauthorized Access.";
                StatusCode = HttpStatusCode.Unauthorized;
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
                StatusCode = HttpStatusCode.InternalServerError;
                ExceptionDetail = GetExceptionDetail(innerException).ToString();
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
