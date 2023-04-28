﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Data.Entities
{
    public class Province
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? RegionId { get; set; }
        public string Description { get; set; }
        public int Effect { get; set; }
        public int OrdinalNumber { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? StartDay { get; set; }
        public DateTime? EndDay { get; set; }
        public string Note { get; set; }
        public List<District> Districts { get; set; }
        public List<AdministrativeUnit> AdministrativeUnits { get; set; }
        public Country Countries { get; set; }
        public Region Regions { get; set; }
        public List<Staff> Staffs { get; set; }
    }
}