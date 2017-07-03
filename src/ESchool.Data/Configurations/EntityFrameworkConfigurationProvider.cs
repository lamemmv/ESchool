using System;
using System.Collections.Generic;
using System.Linq;
using ESchool.Data.Entities.Settings;
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
                    ? SeedSettings(dbContext)
                    : settings.ToDictionary(s => s.Name, s => s.Value);
            }
        }

        private IDictionary<string, string> SeedSettings(ObjectDbContext dbContext)
        {
            var configSettings = new Dictionary<string, string>
            {
                { "account.lockoutonfailure", "true" }
            };

            dbContext.Set<Setting>().AddRange(
                configSettings.Select(kvp => new Setting { Name = kvp.Key, Value = kvp.Value }));

            dbContext.SaveChanges();

            return configSettings;
        }
    }
}
