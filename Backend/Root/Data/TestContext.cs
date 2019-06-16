using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Root.Model;
using Root.Model.Map;

namespace Root.Data
{
    public class TestContext : IdentityDbContext<User>
    {
        //public DbSet<TestModel> Test { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Document> Documents { get; set; }
        //public DbSet<Feature> Features { get; set; }
        //public DbSet<RoleFeature> RoleFeatures { get; set; }
        public TestContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration (new TestModelMap ());
            //modelBuilder.ApplyConfiguration (new UserMap ());
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new DocumentMap());
            //modelBuilder.ApplyConfiguration (new RoleMap ());
            //modelBuilder.ApplyConfiguration (new FeatureMap ());
            //modelBuilder.ApplyConfiguration (new RoleFeatureMap ());
        }

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}