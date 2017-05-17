using ESchool.Data.Mapping;
using ESchool.Domain.Entities.Accounts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Models;

namespace ESchool.Data
{
    public sealed class ObjectDbContext : IdentityDbContext<ApplicationUser>
    {
        public ObjectDbContext(DbContextOptions<ObjectDbContext> options)
            : base(options)
        {
        }

        public DbSet<OpenIddictApplication> OpenIddictApplications
        {
            get
            {
                return Set<OpenIddictApplication>();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            IEntityMapper mapper = new ObjectEntityMapper();
            mapper.MapEntities(builder);
        }
    }
}
