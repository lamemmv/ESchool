using System.Net;

namespace ESchool.Services.Exceptions
{
    public sealed class ApiError
    {
        public ApiError(ErrorCode code)
            : this(code, null, null)
        {
        }

        public ApiError(ErrorCode code, string message)
            : this(code, message, null)
        {
        }

        public ApiError(ErrorCode code, string message, string stackTrace)
        {
            Code = (int)code;
            Message = message;
            StackTrace = stackTrace;
            StatusCode = GetHttpStatusCode(code);
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public int StatusCode { get; }

        private int GetHttpStatusCode(ErrorCode errorCode)
        {
            HttpStatusCode statusCode;

            switch (errorCode)
            {
                case ErrorCode.Unauthorized:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case ErrorCode.NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case ErrorCode.DuplicateEntity:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case ErrorCode.InternalServerError:
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            return (int)statusCode;
        }
    }
}
