using System;

namespace ESchool.Services.Exceptions
{
    public sealed class EntityDuplicateException : Exception
    {
        public EntityDuplicateException(string message)
            : base(message)
        {
        }
    }
}
