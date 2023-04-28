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
    public class AdministrativeUnitConfiguration : IEntityTypeConfiguration<AdministrativeUnit>
    {
        public void Configure(EntityTypeBuilder<AdministrativeUnit> builder)
        {
            builder.ToTable("AdministrativeUnits");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired().IsUnicode(false).HasMaxLength(200);

            builder.Property(x => x.ShortName).HasMaxLength(250);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

            builder.Property(x => x.Description).HasMaxLength(500);

            builder.Property(x => x.Effect).IsRequired();

            builder.Property(x => x.OrdinalNumber).IsRequired();

            builder.Property(x => x.DateCreated).IsRequired();

            builder.Property(x => x.StartDay);

            builder.Property(x => x.EndDay);

            builder.Property(x => x.Note).HasMaxLength(200);

            builder.Property(x => x.UnsignedName).HasMaxLength(250);

            builder.HasOne(x => x.Villages).WithMany(x => x.AdministrativeUnits).HasForeignKey(x => x.VillageId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Districts).WithMany(x => x.AdministrativeUnits).HasForeignKey(x => x.DistrictId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Provinces).WithMany(x => x.AdministrativeUnits).HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Countries).WithMany(x => x.AdministrativeUnits).HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Regions).WithMany(x => x.AdministrativeUnits).HasForeignKey(x => x.RegionId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}