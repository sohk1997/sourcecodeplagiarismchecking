using SourceCodePlagiarismCheckingSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    internal class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(k => k.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.FirstName).IsUnicode().HasMaxLength(250).IsRequired();
            Property(p => p.FirstName).IsUnicode().HasMaxLength(250).IsRequired();
            Property(p => p.DayOfBirth).IsRequired().HasColumnType("date");
            Property(p => p.EmailAddress).HasMaxLength(500).IsOptional();
            Property(p => p.ProfilePicture).IsUnicode().HasMaxLength(1000).IsOptional();
            Property(p => p.CountryId).IsOptional();
            Property(p => p.isActive).IsRequired();
        }
    }
}