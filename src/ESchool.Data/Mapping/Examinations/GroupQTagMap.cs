using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public class GroupQTagMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<GroupQTag>();

            builder.ToTable("GroupQTags", "dbo").HasKey(gt => new { gt.GroupId, gt.QTagId });

            builder.HasOne(gt => gt.Group)
               .WithMany(g => g.GroupQTags)
               .HasForeignKey(gt => gt.GroupId);

            builder.HasOne(gt => gt.QTag)
               .WithMany(t => t.GroupQTags)
               .HasForeignKey(gt => gt.QTagId);
        }
    }
}
