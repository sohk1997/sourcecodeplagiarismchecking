using SourceCodePlagiarismCheckingSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    public class DataConfiguration : EntityTypeConfiguration<Data>
    {
        public DataConfiguration()
        {
            ToTable("Data");
            HasKey(k =>k.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Drink).IsUnicode().HasMaxLength(500).IsRequired();
            Property(p => p.Quantity).IsOptional();
            
        }
    }
}