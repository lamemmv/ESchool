using ESchool.Domain.Entities.Examinations;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Examinations
{
    public sealed class StudentMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Student>();

            builder.ToTable("Students", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(256);

            builder.Property(p => p.LastName).HasMaxLength(256);

            builder.Property(p => p.PhoneNumber).HasMaxLength(32);
        }
    }
}
