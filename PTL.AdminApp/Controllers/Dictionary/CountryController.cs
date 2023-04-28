using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Drawing.Printing;

namespace PTL.AdminApp.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryApiClient _CountryApiClient;
        private readonly IConfiguration _configuration;

        public CountryController(IConfiguration configuration, ICountryApiClient CountryApiClient)
        {
            _configuration = configuration;
            _CountryApiClient = CountryApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _CountryApiClient.GetAllPagings(request);
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
            CountryCreateRequest create = new CountryCreateRequest();
            return PartialView("Create", create);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CountryCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _CountryApiClient.Create(request);
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
            var Country = await _CountryApiClient.GetById(id, languageId);
            var editVm = new CountryUpdateRequest()
            {
                Id = Country.Id,
                Code = Country.Code,
                Name = Country.Name,
                Description = Country.Description,
                OrdinalNumber = Country.OrdinalNumber,
                Effect = Country.Effect,
                DateCreated = Country.DateCreated,
                StartDay = Country.StartDay,
                EndDay = Country.EndDay,
                Note = Country.Note,      
            };
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CountryUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _CountryApiClient.Update(request);
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
            return PartialView("Delete",new CountryDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CountryDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _CountryApiClient.Delete(request.Id);
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
            var result = await _CountryApiClient.GetById(id, languageId);
            return PartialView("Details", result);
        }
    }
}
