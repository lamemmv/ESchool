using System;
using ESchool.Data.Entities.Examinations;

namespace ESchool.Services.Exceptions
{
    public sealed class DuplicateQTagException : Exception
    {
        private readonly QTag _qtag;

        public DuplicateQTagException(QTag entity)
        {
            _qtag = entity;
        }

        public override string Message
        {
            get
            {
                return $"'QTag Name' is duplicated. Parameters: {LogQTag()}";
            }
        }

        private string LogQTag()
        {
            return $"[GroupId] = {_qtag.GroupId}, [ParentId] = {_qtag.ParentId}, [QTagName] = {_qtag.Name}";
        }
    }
}
