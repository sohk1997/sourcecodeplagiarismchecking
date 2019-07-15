using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebClient.Database.Configurations;
using WebClient.Models;

namespace WebClient.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("Name=AppDB") { }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SourceCode> SourceCodes { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SubmissionConfiguration());
            modelBuilder.Configurations.Add(new SourceCodeConfiguration());

            OnModelRelationshipCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        protected void OnModelRelationshipCreating(DbModelBuilder modelBuilder)
        {
           
        }
    }
}