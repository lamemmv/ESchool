using System.Net;

namespace ESchool.Domain.Enums
{
    public enum ErrorCode
    {
        Success = 0,

        NotFound = HttpStatusCode.NotFound,
        BadRequest = HttpStatusCode.BadRequest,
        InternalServerError = HttpStatusCode.InternalServerError,

        DuplicateEntity = 1001,
        InvalidEntityId = 1002
    }
}
