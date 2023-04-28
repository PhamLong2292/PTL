using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PTL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Data.Configurations
{
    public class Provincefiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.ToTable("Provinces");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired().IsUnicode(false).HasMaxLength(200);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);

            builder.HasOne(x => x.Regions).WithMany(x => x.Provinces).HasForeignKey(x => x.RegionId);

            builder.HasOne(x => x.Countries).WithMany(x => x.Provinces).HasForeignKey(x => x.CountryId);

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