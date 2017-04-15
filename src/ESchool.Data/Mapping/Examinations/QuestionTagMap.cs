using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class QuestionTagMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<QuestionTag>();

            builder.ToTable("QuestionTags", "dbo").HasKey(p => p.Id);

            builder.HasOne(qt => qt.QTag)
               .WithMany(t => t.QuestionTags)
               .HasForeignKey(qt => qt.QTagId);

            builder.HasOne(qt => qt.Question)
               .WithMany(t => t.QuestionTags)
               .HasForeignKey(qt => qt.QuestionId);
        }
    }
}
