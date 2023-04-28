using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTL.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Data.Configurations
{
    public class StaffImageConfiguration : IEntityTypeConfiguration<StaffImage>
    {
        public void Configure(EntityTypeBuilder<StaffImage> builder)
        {
            builder.ToTable("StaffImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.ImagePath).HasMaxLength(200).IsRequired(true);

            builder.Property(x => x.Caption).HasMaxLength(200);

            builder.HasOne(x => x.Staffs).WithMany(x => x.StaffImages).HasForeignKey(x => x.StaffId);
        }
    }
}