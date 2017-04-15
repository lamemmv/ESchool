using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class QuestionMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Question>();

            builder.ToTable("Questions", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.Content).IsRequired();
        }
    }
}
