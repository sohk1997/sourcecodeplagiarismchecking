using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Model.Map
{
    public class DocumentMap : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.DocumentId).IsUnique();

            builder.Property(u => u.Id).HasValueGenerator<GuidValueGenerator>().ValueGeneratedOnAdd();
            builder.Property(u => u.DocumentId).UseSqlServerIdentityColumn();
            builder.Property(u => u.DocumentName).HasMaxLength(200);
            builder.Property(u => u.DocumentContent).HasColumnType("varbinary(MAX)");
            builder.Property(u => u.DocumentExtn).HasMaxLength(6);
            builder.Property(u => u.DocumentId).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.ToTable("Document");
        }
    }
}
