using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ESchool.Data.Configurations
{
    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEntityFrameworkConfig(
            this IConfigurationBuilder builder,
            Action<DbContextOptionsBuilder> setup)
        {
            return builder.Add(new EntityFrameworkConfigurationSource(setup));
        }
    }
}
