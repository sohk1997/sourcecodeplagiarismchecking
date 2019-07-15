using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql;


namespace WebAPI.Configuration
{
    public static class MigrationConfigure
    {
        public static void AddMigrationConfigure(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var optionBuilder = new DbContextOptionsBuilder();
            optionBuilder.UseMySql(configuration.GetConnectionString("TestDB"));
            using(var context = serviceProvider.GetService<DbContext>())
            {
                context.Database.Migrate();
            }
        }
    }
}
