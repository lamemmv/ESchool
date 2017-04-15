using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping
{
    public interface IEntityMap
    {
        void Map(ModelBuilder modelBuilder);
    }
}
