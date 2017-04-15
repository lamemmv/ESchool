using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class ExamMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Exam>();

            builder.ToTable("Exams", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }
    }
}
