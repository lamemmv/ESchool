using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class AnswerMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Answer>();

            builder.ToTable("Answers", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.AnswerName).IsRequired().HasMaxLength(32);

            builder.Property(p => p.Body).IsRequired();

            builder.HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId);
        }
    }
}
