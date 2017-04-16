﻿using System.Net;

namespace ESchool.Domain.Enums
{
    public enum ErrorCode
    {
        Success = 0,

        DuplicateEntity = 1,
        InvalidEntityId = 2,

        NotFound = HttpStatusCode.NotFound,
        BadRequest = HttpStatusCode.BadRequest,
        InternalServerError = HttpStatusCode.InternalServerError
    }
}