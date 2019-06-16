using SourceCodePlagiarismCheckingSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    internal class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            ToTable("Countries");
            HasKey(k => k.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.ISO3).IsUnicode().HasMaxLength(3);
            Property(p => p.CountryName).IsUnicode().HasMaxLength(256);
        }
    }
}