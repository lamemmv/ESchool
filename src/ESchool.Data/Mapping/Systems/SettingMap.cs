using ESchool.Data.Entities.Systems;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Systems
{
    public sealed class SettingMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Setting>();

            builder.ToTable("Settings", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);

            builder.Property(p => p.Value).IsRequired().HasMaxLength(256);
        }
    }
}
