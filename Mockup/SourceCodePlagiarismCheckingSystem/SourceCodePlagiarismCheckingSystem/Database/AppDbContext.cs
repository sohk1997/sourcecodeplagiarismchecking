using SourceCodePlagiarismCheckingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("Name=AppDB") { }
        public DbSet<Data> Datas { get; set; }
        public DbSet<SourceCode> SourceCodes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DataConfiguration());
            modelBuilder.Configurations.Add(new SourceCodeConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
            OnModelRelationshipCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }


        protected void OnModelRelationshipCreating(DbModelBuilder modelBuilder)
        {
            //Country->Person
            modelBuilder.Entity<Country>()
                       .HasMany(country => country.Users)
                       .WithOptional(user => user.Country)
                       .HasForeignKey(user => user.CountryId)
                       .WillCascadeOnDelete(false);
        }

    }
}