using System;
using System.Collections.Generic;
using System.Linq;

namespace ESchool.Data.Paginations
{
    public sealed class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> source, int page, int size)
        {
            Page = page;
            Size = size;
            TotalItems = source.Count();

            TotalPages = (int)Math.Ceiling(TotalItems / (double)Size);

            AddRange(source.Skip((Page - 1) * Size).Take(Size));
        }

        public PagedList(IEnumerable<T> source, int page, int size, int totalItems)
        {
            Page = page;
            Size = size;
            TotalItems = totalItems;

            TotalPages = (int)Math.Ceiling(TotalItems / (double)Size);

            AddRange(source);
        }

        public int Page { get; private set; }

		public int Size { get; private set; }

		public int TotalItems { get; private set; }

        public int TotalPages { get; private set; }
    }
}
