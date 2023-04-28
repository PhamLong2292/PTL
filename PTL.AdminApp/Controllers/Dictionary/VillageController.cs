using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;

namespace PTL.AdminApp.Controllers
{
    public class VillageController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IVillageApiClient _villageService;
        private readonly IDistrictApiClient _districtApiClient;

        public VillageController(IConfiguration configuration, IVillageApiClient villageApiClient, IDistrictApiClient districtApiClient)
        {
            _configuration = configuration;
            _villageService = villageApiClient;
            _districtApiClient = districtApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _villageService.GetAllPagings(request);
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
            VillageCreateRequest create = new VillageCreateRequest();
            return PartialView("Create", create);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _villageService.Create(request);
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
            var Village = await _villageService.GetById(id, languageId);
            var editVm = new VillageUpdateRequest()
            {
                Id = Village.Id,
                Code = Village.Code,
                Name = Village.Name,
                DistrictId = Village.DistrictId,
                Description = Village.Description,
                OrdinalNumber = Village.OrdinalNumber,
                Effect = Village.Effect,
                DateCreated = Village.DateCreated,
                StartDay = Village.StartDay,
                EndDay = Village.EndDay,
                Note = Village.Note,      
            };
            if(Village.DistrictId != null)
            {
                var disstrictData = await _districtApiClient.GetById(Village.DistrictId, languageId);
                ViewBag.disstrictsData = disstrictData.Name;
            }    
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] VillageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _villageService.Update(request);
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
            return PartialView("Delete",new VillageDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VillageDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _villageService.Delete(request.Id);
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
            var result = await _villageService.GetById(id, languageId);
            if(result.DistrictId != null)
            {
                var districts = await _districtApiClient.GetById(result.DistrictId, languageId);
                ViewBag.Districts = districts.Name;
            }    
            return PartialView("Details", result);
        }
    }
}
