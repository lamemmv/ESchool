using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class StudentExamPaperResultMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<StudentExamPaperResult>();

            builder.ToTable("StudentExamPaperResults", "dbo").HasKey(p => p.Id);

            builder.HasOne(se => se.Student)
                .WithMany(s => s.StudentExamPaperResults)
                .HasForeignKey(se => se.StudentId);

            builder.HasOne(se => se.ExamPaper)
                .WithMany(e => e.StudentExamPaperResults)
                .HasForeignKey(se => se.ExamPaperId);
        }
    }
}
