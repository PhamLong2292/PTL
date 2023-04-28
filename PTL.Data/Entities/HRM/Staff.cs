using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Data.Entities
{
    public class Staff
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? AdministrativeUnitId { get; set; }
        public string PermanentPlace { get; set; }
        public int? Gender { get; set; }
        public Guid? EthnicId { get; set; }
        public string Religion { get; set; }
        public string NumberIdentityCard { get; set; }
        public DateTime? Date { get; set; }
        public string IssuedBy { get; set; }
        public string Department { get; set; }
        public Guid? WorkUnitId { get; set; }
        public int? NumberYearOfEx { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageContent { get; set; }
        public string ImagePath { get; set; }
        public string Extang { get; set; }
        public Guid? GraduationId { get; set; }
        public int? GraduationYear { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? ExpertiseId { get; set; }
        public string specializations { get; set; }
        public string UserName { get; set; }
        public Guid? CreatorId { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? EditorId { get; set; }
        public DateTime? CorrectionDate { get; set; }
        public AppUser AppUsers { get; set; }
        public AdministrativeUnit AdministrativeUnits { get; set; }
        public Ethnic Ethnics { get; set; }
        public WorkUnit WorkUnit { get; set; }
        public Graduation Graduations { get; set; }
        public Province Provinces { get; set; }
        public Expertise Expertises { get; set; }
        public School Schools { get; set; }
        public List<StaffImage> StaffImages { get; set; }
    }
}
