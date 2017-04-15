using ESchool.Data.Mapping;
using ESchool.Domain.Entities.Systems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

            IEntityMapper mapper = new ObjectEntityMapper();
            mapper.MapEntities(builder);
        }
    }
}
