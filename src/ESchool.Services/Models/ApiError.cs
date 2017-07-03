using System.Collections.Generic;
using System.Linq;
using ESchool.Services.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ESchool.Services.Models
{
    public sealed class ApiError
    {
        public ApiError(ApiErrorCode errorCode, string errorMessage)
            : this(errorCode, errorMessage, null)
        {
        }

        public ApiError(ModelStateDictionary modelState)
            : this(
                  ApiErrorCode.ValidateViewModelFail,
                  null,
                  from kvp in modelState
                  from e in kvp.Value.Errors
                  let k = kvp.Key
                  select new ValidationError(k, e.ErrorMessage))
        {
        }

        private ApiError(
            ApiErrorCode errorCode,
            string errorMessage,
            IEnumerable<ValidationError> validationErrors)
        {
            ErrorCode = (int)errorCode;
            ErrorMessage = errorMessage;
            ValidationErrors = validationErrors;
        }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }

        public IEnumerable<ValidationError> ValidationErrors { get; }
    }
}
