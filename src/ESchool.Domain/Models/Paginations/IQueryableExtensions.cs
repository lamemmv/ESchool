using System.Collections.Generic;
using System.Linq;

namespace ESchool.Domain.Models.Paginations
{
    public static class IQueryableExtensions
	{
		public static IEnumerable<T> ToPagedList<T>(this IQueryable<T> source, int page, int size)
		{
            return new PagedList<T>(source, page, size);
		}

        public static IEnumerable<T> ToPagedList<T>(this IEnumerable<T> source, int page, int size, int totalItems)
        {
            return new PagedList<T>(source, page, size, totalItems);
        }
    }
}
