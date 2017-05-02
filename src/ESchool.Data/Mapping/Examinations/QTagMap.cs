using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class QTagMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<QTag>();

            builder.ToTable("QTags", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);

            builder.HasOne(t => t.Group)
                .WithMany(g => g.QTags)
                .HasForeignKey(t => t.GroupId);
        }
    }
}
