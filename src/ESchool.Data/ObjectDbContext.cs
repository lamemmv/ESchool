using ESchool.DomainModels.Entities.Accounts;
using ESchool.DomainModels.Entities.Logs;
using ESchool.DomainModels.Entities.Settings;
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

            //Map(builder.Entity<Address>());
            //Map(builder.Entity<Menu>());
            //Map(builder.Entity<Restaurant>());
        }

        private void Map(EntityTypeBuilder<Log> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Application).HasMaxLength(64);

            builder.Property(p => p.Level).HasMaxLength(64);

            builder.Property(p => p.Logger).HasMaxLength(256);
        }

        private void Map(EntityTypeBuilder<Setting> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(128);

            builder.Property(p => p.Value).IsRequired().HasMaxLength(128);
        }

        //private void Map(EntityTypeBuilder<Address> builder)
        //{
        //    builder.HasKey(p => p.Id);

        //    builder.Property(p => p.Name).IsRequired().HasMaxLength(128);

        //    builder.Property(p => p.Photo).HasMaxLength(512);
        //}

        //private void Map(EntityTypeBuilder<Menu> builder)
        //{
        //    builder.HasKey(p => p.Id);

        //    builder.Property(p => p.Name).IsRequired().HasMaxLength(128);

        //    builder.Property(p => p.IconName).HasMaxLength(128);
        //}

        //private void Map(EntityTypeBuilder<Restaurant> builder)
        //{
        //    builder.HasKey(p => p.Id);

        //    builder.Property(p => p.Name).IsRequired().HasMaxLength(128);

        //    builder.HasMany(r => r.Addresses).WithOne(a => a.Restaurant);
        //    builder.HasMany(r => r.Menus).WithOne(m => m.Restaurant);
        //}
    }
}
