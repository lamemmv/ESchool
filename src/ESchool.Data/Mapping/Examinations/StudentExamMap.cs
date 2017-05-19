using ESchool.Data.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class StudentExamMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<StudentExam>();

            builder.ToTable("StudentExams", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Comment).HasMaxLength(512);

            builder.HasOne(se => se.Student)
                .WithMany(s => s.StudentExams)
                .HasForeignKey(se => se.StudentId);

            builder.HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId);
        }
    }
}
