using System;

namespace ESchool.Services.Exceptions
{
    public sealed class EntityNotFoundException : Exception 
    {
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(int id, string entityName)
            : base($"{entityName} not found. Id = {id}")
        {
        }
    }
}
