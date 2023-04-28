using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.AdminApp.Models;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;
using System.Drawing;

namespace PTL.AdminApp.Controllers
{
    public class ProvinceController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IProvinceApiClient _provinceApiClient;
        private readonly ICountryApiClient _countryApiClient;
        private readonly IRegionApiClient _regionApiClient;

        public ProvinceController( IConfiguration configuration, IProvinceApiClient ProvinceApiClient, ICountryApiClient countryApiClient, IRegionApiClient regionApiClient)
        {
            _configuration = configuration;
            _provinceApiClient = ProvinceApiClient;
            _countryApiClient = countryApiClient;
            _regionApiClient = regionApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };         
            var data = await _provinceApiClient.GetAllPagings(request);
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
            ProvinceCreateRequest create = new ProvinceCreateRequest();                     
            return PartialView("Create", create);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProvinceCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _provinceApiClient.Create(request);
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
            var Province = await _provinceApiClient.GetById(id, languageId);
            var editVm = new ProvinceUpdateRequest()
            {
                Id = Province.Id,
                Code = Province.Code,
                Name = Province.Name,
                CountryId = Province.CountryId,
                RegionId = Province.RegionId,
                Description = Province.Description,
                OrdinalNumber = Province.OrdinalNumber,
                Effect = Province.Effect,
                DateCreated = Province.DateCreated,
                StartDay = Province.StartDay,
                EndDay = Province.EndDay,
                Note = Province.Note,      
            };
            if(Province.CountryId != null)
            {
                var countriesData = await _countryApiClient.GetById(Province.CountryId, languageId);
                ViewBag.countriesData = countriesData.Name;
            }    
            if(Province.RegionId != null)
            {
                var RegionData = await _regionApiClient.GetById(Province.RegionId, languageId);
                ViewBag.RegionData = RegionData.Name;
            }
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ProvinceUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _provinceApiClient.Update(request);
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
            return PartialView("Delete",new ProvinceDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProvinceDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _provinceApiClient.Delete(request.Id);
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
            var result = await _provinceApiClient.GetById(id, languageId);
            if(result.RegionId != null)
            {
                var regions = await _regionApiClient.GetById(result.RegionId, languageId);
                ViewBag.Regions = regions.Name;
            }
            if (result.CountryId != null)
            {
                var countries = await _countryApiClient.GetById(result.CountryId, languageId);
                ViewBag.Countries = countries.Name;
            }               
            return PartialView("Details", result);
        }
    }
}
