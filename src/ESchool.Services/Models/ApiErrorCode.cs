using System.Net;

namespace ESchool.Services.Models
{
    public enum ApiErrorCode
    {
        Undefined = 0,

        Unauthorized = HttpStatusCode.Unauthorized,
        NotFound = HttpStatusCode.NotFound,
        InternalServerError = HttpStatusCode.InternalServerError,

        ValidateViewModelFail = 901,
        DuplicateEntity = 1001,
        RandomExamPaperError
    }
}
