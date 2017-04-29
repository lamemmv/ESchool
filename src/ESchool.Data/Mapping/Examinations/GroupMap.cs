using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class GroupMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Group>();

            builder.ToTable("Groups", "dbo").HasKey(p => p.Id);

            builder.Property(g => g.Name).IsRequired().HasMaxLength(128);
        }
    }
}
