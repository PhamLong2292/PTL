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
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("Staffs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).IsRequired().IsUnicode(false).HasMaxLength(200);

            builder.Property(x => x.LastName).HasMaxLength(250);

            builder.Property(x => x.FirstName).HasMaxLength(250);

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(250);

            builder.Property(x => x.DateOfBirth).IsRequired();

            builder.Property(x => x.PermanentPlace).HasMaxLength(200);

            builder.Property(x => x.Gender);

            builder.Property(x => x.Religion);

            builder.Property(x => x.NumberIdentityCard).HasMaxLength(36);

            builder.Property(x => x.Date);

            builder.Property(x => x.IssuedBy);

            builder.Property(x => x.DateCreated).IsRequired();

            builder.Property(x => x.Department);

            builder.Property(x => x.NumberYearOfEx);

            builder.Property(x => x.PhoneNumber).HasMaxLength(11);

            builder.Property(x => x.Email).HasMaxLength(200);

            builder.Property(x => x.ImageContent);

            builder.Property(x => x.ImagePath);

            builder.Property(x => x.GraduationYear);

            builder.Property(x => x.specializations);

            builder.Property(x => x.UserName);

            builder.HasOne(x => x.AppUsers).WithMany(x => x.Staffs).HasForeignKey(x => x.CreatorId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AppUsers).WithMany(x => x.Staffs).HasForeignKey(x => x.EditorId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AdministrativeUnits).WithMany(x => x.Staffs).HasForeignKey(x => x.AdministrativeUnitId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Ethnics).WithMany(x => x.Staffs).HasForeignKey(x => x.EthnicId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.WorkUnit).WithMany(x => x.Staffs).HasForeignKey(x => x.WorkUnitId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Graduations).WithMany(x => x.Staffs).HasForeignKey(x => x.GraduationId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Provinces).WithMany(x => x.Staffs).HasForeignKey(x => x.ProvinceId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Expertises).WithMany(x => x.Staffs).HasForeignKey(x => x.ExpertiseId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Schools).WithMany(x => x.Staffs).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}