using System.Net;

namespace ESchool.Services.Exceptions
{
    public enum ApiErrorCode
    {
        Undefined = 0,

        Unauthorized = HttpStatusCode.Unauthorized,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError,

        DuplicateEntity = 1001
    }
}
