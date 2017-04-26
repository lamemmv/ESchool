using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ESchool.Admin.Attributes
{
    public sealed class ValidateMimeMultipartContentFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsMultipartContentType(context.HttpContext.Request.ContentType))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.UnsupportedMediaType);
                return;
            }

            base.OnActionExecuting(context);
        }

        private static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
