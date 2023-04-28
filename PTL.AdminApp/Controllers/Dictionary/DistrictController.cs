using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;

namespace PTL.AdminApp.Controllers
{
    public class DistrictController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDistrictApiClient _districtApiClient;
        private readonly IProvinceApiClient _provinceApiClient;

        public DistrictController(IConfiguration configuration, IDistrictApiClient districtApiClient, IProvinceApiClient provinceApiClient)
        {
            _configuration = configuration;
            _districtApiClient = districtApiClient;
            _provinceApiClient = provinceApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _districtApiClient.GetAllPagings(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            DistrictCreateRequest create = new DistrictCreateRequest();
            return PartialView("Create", create);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DistrictCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _districtApiClient.Create(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var District = await _districtApiClient.GetById(id, languageId);
            var editVm = new DistrictUpdateRequest()
            {
                Id = District.Id,
                Code = District.Code,
                Name = District.Name,
                ProvinceId = District.ProvinceId,
                Description = District.Description,
                OrdinalNumber = District.OrdinalNumber,
                Effect = District.Effect,
                DateCreated = District.DateCreated,
                StartDay = District.StartDay,
                EndDay = District.EndDay,
                Note = District.Note,      
            };
            if( District.ProvinceId != null)
            {
                var provincesData = await _provinceApiClient.GetById(District.ProvinceId, languageId);
                ViewBag.provincesData = provincesData.Name;
            }
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] DistrictUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _districtApiClient.Update(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật thất bại");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return PartialView("Delete",new DistrictDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DistrictDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _districtApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Xóa người dùng thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var result = await _districtApiClient.GetById(id, languageId);
            if(result.ProvinceId != null)
            {
                var provinces = await _provinceApiClient.GetById(result.ProvinceId, languageId);
                ViewBag.Provinces = provinces.Name;
            }
            return PartialView("Details", result);
        }
    }
}
