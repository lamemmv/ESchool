using System;

namespace ESchool.Services.Constants
{
    public static class ValidationRules
    {
        public static readonly DateTime MinDate = new DateTime(2015, 1, 1);
        public static readonly DateTime MaxDate = new DateTime(2115, 1, 1);

        public const int MinPasswordLength = 6;
    }
}
