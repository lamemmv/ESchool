using System;

namespace ESchool.Services.Exceptions
{
    public sealed class RandomExamPaperException : Exception
    {
        public RandomExamPaperException(string message)
            : base(message)
        {
        }
    }
}
