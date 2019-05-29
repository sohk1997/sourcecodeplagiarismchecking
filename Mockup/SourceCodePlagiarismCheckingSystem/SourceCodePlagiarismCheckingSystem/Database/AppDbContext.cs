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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DataConfiguration());
            modelBuilder.Configurations.Add(new SourceCodeConfiguration());
            OnModelRelationshipCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }


        protected void OnModelRelationshipCreating(DbModelBuilder modelBuilder)
        { }

    }
}