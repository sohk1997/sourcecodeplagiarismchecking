using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using WebClient.Models;

namespace WebClient.Database
{
    internal class SubmissionConfiguration : EntityTypeConfiguration<Submission>
    {
        public SubmissionConfiguration()
        {
            ToTable("Submission");
            HasKey(k => k.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Name).IsUnicode().HasMaxLength(500).IsRequired();
            
        }
    }
}