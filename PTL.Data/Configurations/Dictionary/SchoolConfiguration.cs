﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PTL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Data.Configurations
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("Schools");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired().IsUnicode(false).HasMaxLength(200);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.Property(x => x.Description).HasMaxLength(500);

            builder.Property(x => x.Effect).IsRequired();

            builder.Property(x => x.OrdinalNumber).IsRequired();

            builder.Property(x => x.DateCreated).IsRequired();

            builder.Property(x => x.StartDay);

            builder.Property(x => x.EndDay);

            builder.Property(x => x.Note).HasMaxLength(200);
        }
    }
}