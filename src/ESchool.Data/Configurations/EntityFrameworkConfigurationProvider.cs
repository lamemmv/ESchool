using System;
using System.Linq;
using ESchool.DomainModels.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ESchool.Data.Configurations
{
    public class EntityFrameworkConfigurationProvider : ConfigurationProvider
    {
        protected Action<DbContextOptionsBuilder> OptionsAction { get; }

        public EntityFrameworkConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<ObjectDbContext>();
            OptionsAction(builder);

            using (var dbContext = new ObjectDbContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();
                var settings = dbContext.Set<Setting>();

                Data = !settings.Any()
                    ? new DbInitializer().SeedSettings(dbContext)
                    : settings.ToDictionary(s => s.Name, s => s.Value);
            }
        }
    }
}
