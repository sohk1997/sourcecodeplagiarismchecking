using SourceCodePlagiarismCheckingSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SourceCodePlagiarismCheckingSystem.Database
{
    public class SourceCodeConfiguration : EntityTypeConfiguration<SourceCode>
    {
        public SourceCodeConfiguration()
        {
            ToTable("SourceCode");
            HasKey(k => k.Id);
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.Name).IsUnicode().HasMaxLength(500).IsRequired();
            Property(p => p.Code).IsUnicode().IsOptional().HasColumnType("text");
            Property(p => p.StartLine).IsOptional();
            Property(p => p.EndLine).IsOptional();
            Property(p => p.Description).IsUnicode().IsOptional().HasMaxLength(4000);

        }
    }
}