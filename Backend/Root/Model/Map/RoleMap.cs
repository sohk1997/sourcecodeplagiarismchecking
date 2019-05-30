using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Root.Model.Map {
    public class RoleMap : IEntityTypeConfiguration<IdentityRole> {
        public void Configure (EntityTypeBuilder<IdentityRole> builder) {
            builder.ToTable ("Role");
        }
    }
}