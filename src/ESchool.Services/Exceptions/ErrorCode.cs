using System.Net;

namespace ESchool.Services.Exceptions
{
    public enum ErrorCode
    {
        Undefined = 0,

        Unauthorized = HttpStatusCode.Unauthorized,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError,

        DuplicateEntity = 1001,
        InvalidEntityId = 1002
    }
}
