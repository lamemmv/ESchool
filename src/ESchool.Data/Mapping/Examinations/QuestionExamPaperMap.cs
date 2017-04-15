using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class QuestionExamPaperMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<QuestionExamPaper>();

            builder.ToTable("QuestionExamPapers", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Comment).HasMaxLength(512);

            builder.HasOne(qe => qe.ExamPaper)
               .WithMany(e => e.QuestionExamPapers)
               .HasForeignKey(qe => qe.ExamPaperId);

            builder.HasOne(qe => qe.Question)
               .WithMany(q => q.QuestionExamPapers)
               .HasForeignKey(qe => qe.QuestionId);
        }
    }
}
