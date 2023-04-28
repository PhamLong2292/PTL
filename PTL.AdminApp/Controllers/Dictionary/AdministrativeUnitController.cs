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
using System.Xml.Linq;

namespace PTL.AdminApp.Controllers
{
    public class AdministrativeUnitController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAdministrativeUnitApiClient _administrativeUnitApiClient;
        private readonly IVillageApiClient _villageApiClient;
        private readonly IDistrictApiClient _districtApiClient;
        private readonly IProvinceApiClient _provinceApiClient;
        private readonly ICountryApiClient _countryApiClient;
        private readonly IRegionApiClient _regionApiClient;

        public AdministrativeUnitController( IConfiguration configuration, IAdministrativeUnitApiClient administrativeUnitApiClient, IVillageApiClient villageApiClient, IDistrictApiClient districtApiClient, IProvinceApiClient provinceApiClient, ICountryApiClient countryApiClient, IRegionApiClient regionApiClient)
        {
            _configuration = configuration;
            _administrativeUnitApiClient = administrativeUnitApiClient;
            _villageApiClient = villageApiClient;
            _districtApiClient = districtApiClient;
            _provinceApiClient = provinceApiClient;
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
            var data = await _administrativeUnitApiClient.GetAllPagings(request);
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
            AdministrativeUnitCreateRequest create = new AdministrativeUnitCreateRequest();                     
            return PartialView("Create", create);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdministrativeUnitCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var villages = await _villageApiClient.GetById(request.VillageId, languageId);
            var districts = await _districtApiClient.GetById(request.DistrictId, languageId);
            var provinces = await _provinceApiClient.GetById(request.ProvinceId, languageId);
            var countries = await _countryApiClient.GetById(request.CountryId, languageId);
            var createVm = new AdministrativeUnitCreateRequest()
            {
                Id = request.Id,
                Code = request.Code,
                ShortName = provinces.Name,
                Name = string.Concat(villages.Name, '-', districts.Name, '-', provinces.Name, '-', countries.Name),
                VillageId = request.VillageId,
                DistrictId = request.DistrictId,
                ProvinceId = request.ProvinceId,
                CountryId = request.CountryId,
                RegionId = request.RegionId,
                Description = request.Description,
                OrdinalNumber = request.OrdinalNumber,
                Effect = request.Effect,
                DateCreated = request.DateCreated,
                StartDay = request.StartDay,
                EndDay = request.EndDay,
                Note = request.Note,
                UnsignedName = string.Concat(villages.Code, '-', districts.Code, '-', provinces.Code, '-', countries.Code),
            };
            var result = await _administrativeUnitApiClient.Create(createVm);
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
            var AdministrativeUnit = await _administrativeUnitApiClient.GetById(id, languageId);
            var editVm = new AdministrativeUnitUpdateRequest()
            {
                Id = AdministrativeUnit.Id,
                Code = AdministrativeUnit.Code,
                ShortName = AdministrativeUnit.ShortName,
                Name = AdministrativeUnit.Name,
                VillageId = AdministrativeUnit.VillageId,
                DistrictId = AdministrativeUnit.DistrictId,
                ProvinceId = AdministrativeUnit.ProvinceId,
                CountryId = AdministrativeUnit.CountryId,
                RegionId = AdministrativeUnit.RegionId,
                Description = AdministrativeUnit.Description,
                OrdinalNumber = AdministrativeUnit.OrdinalNumber,
                Effect = AdministrativeUnit.Effect,
                DateCreated = AdministrativeUnit.DateCreated,
                StartDay = AdministrativeUnit.StartDay,
                EndDay = AdministrativeUnit.EndDay,
                Note = AdministrativeUnit.Note,    
                UnsignedName = AdministrativeUnit.UnsignedName,
            };
            ViewBag.villagesData = "";
            if (AdministrativeUnit.VillageId != null)
            {
                var villagesData = await _villageApiClient.GetById(AdministrativeUnit.VillageId, languageId);
                ViewBag.villagesData = villagesData.Name;
            }
            ViewBag.districtsData = "";
            if (AdministrativeUnit.DistrictId != null)
            {
                var districtsData = await _districtApiClient.GetById(AdministrativeUnit.DistrictId, languageId);
                ViewBag.districtsData = districtsData.Name;
            }
            ViewBag.provincesData = "";
            if (AdministrativeUnit.ProvinceId != null)
            {
                var provincesData = await _provinceApiClient.GetById(AdministrativeUnit.ProvinceId, languageId);
                ViewBag.provincesData = provincesData.Name;
            }
            ViewBag.countriesData = "";
            if (AdministrativeUnit.CountryId != null)
            {
                var countriesData = await _countryApiClient.GetById(AdministrativeUnit.CountryId, languageId);
                ViewBag.countriesData = countriesData.Name;
            }
            ViewBag.RegionData = "";
            if (AdministrativeUnit.RegionId != null)
            {
                var regionsData = await _regionApiClient.GetById(AdministrativeUnit.RegionId, languageId);
                ViewBag.regionsData = regionsData.Name;
            }
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] AdministrativeUnitUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var villages = await _villageApiClient.GetById(request.VillageId, languageId);
            var districts = await _districtApiClient.GetById(request.DistrictId, languageId);
            var provinces = await _provinceApiClient.GetById(request.ProvinceId, languageId);
            var countries = await _countryApiClient.GetById(request.CountryId, languageId);
            var editVm = new AdministrativeUnitUpdateRequest()
            {
                Id = request.Id,
                Code = request.Code,
                ShortName = provinces.Name,
                Name = string.Concat(villages.Name, '-', districts.Name, '-', provinces.Name, '-', countries.Name),
                VillageId = request.VillageId,
                DistrictId = request.DistrictId,
                ProvinceId = request.ProvinceId,
                CountryId = request.CountryId,
                RegionId = request.RegionId,
                Description = request.Description,
                OrdinalNumber = request.OrdinalNumber,
                Effect = request.Effect,
                DateCreated = request.DateCreated,
                StartDay = request.StartDay,
                EndDay = request.EndDay,
                Note = request.Note,
                UnsignedName = string.Concat(villages.Code, '-', districts.Code, '-', provinces.Code, '-', countries.Code),
            };
            var result = await _administrativeUnitApiClient.Update(editVm);
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
            return PartialView("Delete",new AdministrativeUnitDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AdministrativeUnitDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _administrativeUnitApiClient.Delete(request.Id);
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
            var result = await _administrativeUnitApiClient.GetById(id, languageId);
            if(result.VillageId != null)
            {
                var villages = await _villageApiClient.GetById(result.VillageId, languageId);
                ViewBag.villages = villages.Name;
            }
            if (result.DistrictId != null)
            {
                var districts = await _districtApiClient.GetById(result.DistrictId, languageId);
                ViewBag.districts = districts.Name;
            }
            if (result.ProvinceId != null)
            {
                var provinces = await _provinceApiClient.GetById(result.ProvinceId, languageId);
                ViewBag.provinces = provinces.Name;
            }
            if (result.RegionId != null)
            {
                var regions = await _regionApiClient.GetById(result.RegionId, languageId);
                ViewBag.regions = regions.Name;
            }
            if (result.VillageId != null)
            {
                var countries = await _countryApiClient.GetById(result.CountryId, languageId);
                ViewBag.countries = countries.Name;
            }           
            return PartialView("Details", result);
        }
    }
}
