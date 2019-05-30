using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Model.Map
{
    public class TestModelMap : IEntityTypeConfiguration<TestModel>
    {
        public void Configure(EntityTypeBuilder<TestModel> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Content).HasMaxLength(1000);
            builder.ToTable("Test");
        }
    }
}
