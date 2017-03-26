using System.Collections.Generic;
using System.Linq;

namespace ESchool.Data.Paginations
{
    public static class IQueryableExtensions
	{
		public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int page, int size)
		{
            return new PagedList<T>(source, page, size);
		}

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int page, int size, int totalItems)
        {
            return new PagedList<T>(source, page, size, totalItems);
        }
    }
}
