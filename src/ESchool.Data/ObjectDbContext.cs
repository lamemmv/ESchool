using ESchool.Domain.Entities.Examinations;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ESchool.Data
{
    public sealed class ObjectDbContext : IdentityDbContext<ApplicationUser>
    {
        public ObjectDbContext(DbContextOptions<ObjectDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            Map(builder.Entity<Log>());
            Map(builder.Entity<Setting>());

            // Examinations.
            Map(builder.Entity<QuestionTag>());
            Map(builder.Entity<QTag>());
            Map(builder.Entity<Question>());
            Map(builder.Entity<Answer>());
            Map(builder.Entity<ExamPaper>());
            Map(builder.Entity<QuestionExamPaper>());
            Map(builder.Entity<Student>());
            Map(builder.Entity<Exam>());
            Map(builder.Entity<StudentExam>());
            Map(builder.Entity<StudentExamPaperResult>());
        }

        #region Systems

        private void Map(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs").HasKey(p => p.Id);

            builder.Property(p => p.Application).HasMaxLength(256);

            builder.Property(p => p.Level).HasMaxLength(256);

            builder.Property(p => p.Logger).HasMaxLength(256);
        }

        private void Map(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Settings").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);

            builder.Property(p => p.Value).IsRequired().HasMaxLength(256);
        }

        #endregion

        #region Examinations

        private void Map(EntityTypeBuilder<QuestionTag> builder)
        {
            builder.ToTable("QuestionTags").HasKey(p => p.Id);

            builder.HasOne(qt => qt.QTag)
               .WithMany(t => t.QuestionTags)
               .HasForeignKey(qt => qt.QTagId);

            builder.HasOne(qt => qt.Question)
               .WithMany(t => t.QuestionTags)
               .HasForeignKey(qt => qt.QuestionId);
        }

        private void Map(EntityTypeBuilder<QTag> builder)
        {
            builder.ToTable("QTags").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }

        private void Map(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions").HasKey(p => p.Id);

            builder.Property(p => p.Content).IsRequired();
        }

        private void Map(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers").HasKey(p => p.Id);

            builder.Property(p => p.AnswerName).IsRequired().HasMaxLength(32);

            builder.Property(p => p.Body).IsRequired();

            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);
        }

        private void Map(EntityTypeBuilder<ExamPaper> builder)
        {
            builder.ToTable("ExamPapers").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }

        private void Map(EntityTypeBuilder<QuestionExamPaper> builder)
        {
            builder.ToTable("QuestionExamPapers").HasKey(p => p.Id);

            builder.Property(p => p.Comment).HasMaxLength(512);

            builder.HasOne(qe => qe.ExamPaper)
               .WithMany(e => e.QuestionExamPapers)
               .HasForeignKey(qe => qe.ExamPaperId);

            builder.HasOne(qe => qe.Question)
               .WithMany(q => q.QuestionExamPapers)
               .HasForeignKey(qe => qe.QuestionId);
        }

        private void Map(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students").HasKey(p => p.Id);

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(256);

            builder.Property(p => p.LastName).HasMaxLength(256);

            builder.Property(p => p.PhoneNumber).HasMaxLength(32);
        }

        private void Map(EntityTypeBuilder<Exam> builder)
        {
            builder.ToTable("Exams").HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(256);
        }

        private void Map(EntityTypeBuilder<StudentExam> builder)
        {
            builder.ToTable("StudentExams").HasKey(p => p.Id);

            builder.Property(p => p.Comment).HasMaxLength(512);

            builder.HasOne(se => se.Student)
                .WithMany(s => s.StudentExams)
                .HasForeignKey(se => se.StudentId);

            builder.HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId);
        }

        private void Map(EntityTypeBuilder<StudentExamPaperResult> builder)
        {
            builder.ToTable("StudentExamPaperResults").HasKey(p => p.Id);

            builder.HasOne(se => se.Student)
                .WithMany(s => s.StudentExamPaperResults)
                .HasForeignKey(se => se.StudentId);

            builder.HasOne(se => se.ExamPaper)
                .WithMany(e => e.StudentExamPaperResults)
                .HasForeignKey(se => se.ExamPaperId);
        }

        #endregion
    }
}
