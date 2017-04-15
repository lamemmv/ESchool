using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Paginations
{
    public static class QueryableExtensions
    {
        public static async Task<IPagedList<T>> GetListAsync<T>(this IQueryable<T> queryable, int page, int size)
        {
            int totalItems = await queryable.CountAsync();

            if (totalItems == 0)
            {
                return new PagedList<T>(page, size);
            }

            var pagedList = await queryable.Skip((page - 1) * size).Take(size).ToListAsync();

            return new PagedList<T>(pagedList, totalItems, page, size);
        }
    }
}
