using ESchool.Data.Entities.Files;
using Microsoft.EntityFrameworkCore;

namespace ESchool.Data.Mapping.Files
{
    public sealed class BlobMap : IEntityMap
    {
        public void Map(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Blob>();

            builder.ToTable("Blobs", "dbo").HasKey(p => p.Id);

            builder.Property(p => p.FileName).IsRequired().HasMaxLength(512);

            builder.Property(p => p.ContentType).IsRequired().HasMaxLength(64);

            builder.Property(p => p.Path).IsRequired().HasMaxLength(512);
        }
    }
}
