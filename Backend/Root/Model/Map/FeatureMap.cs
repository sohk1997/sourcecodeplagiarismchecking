using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Root.Model.Map {
    public class FeatureMap : IEntityTypeConfiguration<Feature> {
        public void Configure (EntityTypeBuilder<Feature> builder) {
            builder.HasKey (c => c.Id);
            builder.HasIndex (c => c.FeatureId).IsUnique ();
            builder.HasIndex (c => c.FeatureCode).IsUnique ();

            builder.Property (u => u.Id).HasValueGenerator<GuidValueGenerator> ().ValueGeneratedOnAdd ();
            builder.Property (u => u.FeatureId).UseSqlServerIdentityColumn ();
            builder.Property (u => u.FeatureName).HasMaxLength (200);
            builder.Property (u => u.FeatureCode).IsRequired (required: true);
            builder.Property (u => u.FeatureId).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.ToTable ("Feature");

            builder.HasData (new Feature () {
                    Id = Guid.NewGuid(),
                    FeatureId = 1,
                    FeatureCode = "CUSTOMER_C",
                    FeatureName = "Customer create"
                },
                new Feature () {
                    Id = Guid.NewGuid(),
                    FeatureId = 2,
                    FeatureCode = "CUSTOMER_V",
                    FeatureName = "Customer view"
                },
                new Feature () {
                    Id = Guid.NewGuid(),
                    FeatureId = 3,
                    FeatureCode = "CUSTOMER_U",
                    FeatureName = "Customer update"
                },
                new Feature () {
                    Id = Guid.NewGuid(),
                    FeatureId = 4,
                    FeatureCode = "CUSTOMER_D",
                    FeatureName = "Customer delete"
                });
        }
    }
}