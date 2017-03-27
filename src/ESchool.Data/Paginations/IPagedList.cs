using System.Collections.Generic;

namespace ESchool.Data.Paginations
{
    public interface IPagedList<T> : IEnumerable<T>
    {
        int Page { get; }

        int Size { get; }

        int TotalItems { get; }

        int TotalPages { get; }
    }
}
