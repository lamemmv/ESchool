using ESchool.Data.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class ExamPaperMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<ExamPaper>();

            builder.ToTable("ExamPapers", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }
    }
}
