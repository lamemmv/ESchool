using System.Collections.Generic;

namespace ESchool.Data.Paginations
{
    public interface IPagedList<T>
    {
        IEnumerable<T> Data { get; }

        int Page { get; }

        int Size { get; }

        int TotalItems { get; }

        int TotalPages { get; }
    }
}
