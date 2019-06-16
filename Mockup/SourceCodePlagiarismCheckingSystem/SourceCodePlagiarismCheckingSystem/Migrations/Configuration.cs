namespace SourceCodePlagiarismCheckingSystem.Migrations
{
    using SourceCodePlagiarismCheckingSystem.Database;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SourceCodePlagiarismCheckingSystem.Database.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SourceCodePlagiarismCheckingSystem.Database.AppDbContext context)
        {
            new List<EntitySeeder>()
            {
                new CountrySeeder(),
                new UserSeeder(),
                //new ITAssetSeeder(),
            }
            .ForEach(x =>
            {
                x.Seed(context);
            });
        }
    }
}
