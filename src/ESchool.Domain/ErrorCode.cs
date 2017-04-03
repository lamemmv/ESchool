using System.Net;

namespace ESchool.Domain
{
    public enum ErrorCode
    {
        Success = 0,

        DuplicateEntity = 1,
        InvalidIdEntity = 2,

        NotFound = HttpStatusCode.NotFound,
        BadRequest = HttpStatusCode.BadRequest
    }
}
