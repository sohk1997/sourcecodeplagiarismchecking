using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Root.Model.Map {
    public class CustomerMap : IEntityTypeConfiguration<Customer> {
        public void Configure (EntityTypeBuilder<Customer> builder) {
            builder.HasKey (c => c.Id);
            builder.HasIndex (c => c.CustomerId).IsUnique ();

            builder.Property (u => u.Id).HasValueGenerator<GuidValueGenerator> ().ValueGeneratedOnAdd ();
            builder.Property (u => u.CustomerId).UseSqlServerIdentityColumn ();
            builder.Property (u => u.CustomerName).HasMaxLength (200);
            builder.Property (u => u.Status).IsUnicode (false).HasDefaultValue ("1");
            builder.Property (u => u.CreatedDate).ValueGeneratedOnAdd ().HasDefaultValue (new DateTime ());
            builder.Property (u => u.UpdatedDate).ValueGeneratedOnAdd ().HasDefaultValue (new DateTime ());
            builder.Property (u => u.CustomerId).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.ToTable ("Customer");
        }
    }
}