using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping
{
    public interface IEntityMapper
    {
        IEnumerable<IEntityMap> Mappings { get; }

        void MapEntities(ModelBuilder modelBuilder);
    }
}
