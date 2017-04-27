using System.Net;

namespace ESchool.Services.Exceptions
{
    public enum ErrorCode
    {
        Unauthorized = HttpStatusCode.Unauthorized,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError,

        DuplicateEntity = 1001,
        InvalidEntityId = 1002
    }
}
