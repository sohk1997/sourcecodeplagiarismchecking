using Microsoft.EntityFrameworkCore;

namespace WebCheck{
    public class MyContext : DbContext
    {
        public DbSet<Submission> SourceCode { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            optionsBuilder.UseMySql(@"Server=35.198.247.133;Database=SourceCodePlagiarism;Uid=root;Pwd=1234567890;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Submission>().ToTable("Submission");
            base.OnModelCreating(builder);
        }
    }
}