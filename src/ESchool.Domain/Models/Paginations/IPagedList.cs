using System.Collections.Generic;

namespace ESchool.Domain.Models.Paginations
{
    public interface IPagedList<T> : IEnumerable<T>
    {
        int Page { get; }

        int Size { get; }

        int TotalItems { get; }

        int TotalPages { get; }
    }
}
