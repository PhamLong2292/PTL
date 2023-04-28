using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Data.Entities
{
    public class AdministrativeUnit
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string Description{ get; set; }
        public int Effect { get; set; }
        public int OrdinalNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public string Note { get; set; }
        public Guid? VillageId { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? RegionId { get; set; }
        public string UnsignedName { get; set; }
        public Village Villages { get; set; }
        public District Districts { get; set; }
        public Province Provinces { get; set; }
        public Country Countries { get; set; }
        public Region Regions { get; set; }
        public List<Staff> Staffs { get; set; }
    }
}
