using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PTL.ViewModels
{
    public class AdministrativeUnitCreateRequest
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Stt")]
        [Required]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Mã")]
        [Required]
        public string Code { get; set; }

        [Display(Name = "Tên tắt")]
        public string ShortName { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Xã Phường")]
        public Guid? VillageId { get; set; }

        [Display(Name = "Quận Huyện")]
        public Guid? DistrictId { get; set; }

        [Display(Name = "Tỉnh Thành")]
        public Guid? ProvinceId { get; set; }

        [Display(Name = "Quốc gia")]
        public Guid? CountryId { get; set; }

        [Display(Name = "Vùng miền")]
        public Guid? RegionId { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Hiệu lực")]
        public int Effect { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Display(Name = "Từ ngày")]
        public DateTime? StartDay { get; set; }

        [Display(Name = "Đến ngày")]
        public DateTime? EndDay { get; set; }

        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Display(Name = "Tên không dấu")]
        public string UnsignedName { get; set; }
    }
}
