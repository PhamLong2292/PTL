using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PTL.ViewModels
{
    public class WorkUnitVm
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Stt")]
        public int OrdinalNumber { get; set; }

        [Display(Name = "Mã")]
        public string Code { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

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
    }
}