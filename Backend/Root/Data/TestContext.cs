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
    public class TestContext : DbContext
    {
        //public DbSet<TestModel> Test { get; set; }
        public DbSet<Submission> Documents { get; set; }
        public DbSet<CodeDetail> Methods { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<User> User { get; set; }
        //public DbSet<Feature> Features { get; set; }
        //public DbSet<RoleFeature> RoleFeatures { get; set; }
        public TestContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Submission>().ToTable("Submission");
            builder.Entity<Result>().ToTable("Result");
            builder.Entity<CodeDetail>().ToTable("CodeDetail");
            base.OnModelCreating(builder);
        }

        public void Commit()
        {
            this.SaveChanges();
        }
    }
}