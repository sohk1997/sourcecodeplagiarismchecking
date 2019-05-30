using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Root.Model.Map {
    public class RoleFeatureMap : IEntityTypeConfiguration<RoleFeature> {
        public void Configure (EntityTypeBuilder<RoleFeature> builder) {
            builder.HasKey (c => c.Id);
            builder.HasIndex (u => u.RoleId);
            builder.HasIndex (u => u.FeatureId);

            builder.Property (u => u.Id).HasValueGenerator<GuidValueGenerator> ().ValueGeneratedOnAdd ();
            builder.Property (u => u.FeatureId).IsRequired (required: true);
            builder.Property (u => u.RoleId).IsRequired (required: true);

            builder.ToTable ("RoleFeature");

            builder.HasData(new RoleFeature(){
                Id = Guid.NewGuid(),
                FeatureId = 1,
                RoleId = "1"
            });
            
            builder.HasData(new RoleFeature(){
                Id = Guid.NewGuid(),
                FeatureId = 2,
                RoleId = "1"
            });

            builder.HasData(new RoleFeature(){
                Id = Guid.NewGuid(),
                FeatureId = 3,
                RoleId = "1"
            });

            builder.HasData(new RoleFeature(){
                Id = Guid.NewGuid(),
                FeatureId = 4,
                RoleId = "1"
            });

            builder.HasData(new RoleFeature(){
                Id = Guid.NewGuid(),
                FeatureId = 2,
                RoleId = "2"
            });
        }
    }
}