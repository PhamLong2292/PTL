using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;

namespace PTL.AdminApp.Controllers
{
    public class SchoolController : Controller
    {
        private readonly ISchoolApiClient _schoolApiClient;
        private readonly IConfiguration _configuration;

        public SchoolController(IConfiguration configuration, ISchoolApiClient schoolApiClient)
        {
            _configuration = configuration;
            _schoolApiClient = schoolApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _schoolApiClient.GetAllPagings(request);
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
            SchoolCreateRequest create = new SchoolCreateRequest();
            return PartialView("Create", create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SchoolCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _schoolApiClient.Create(request);
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
            var school = await _schoolApiClient.GetById(id, languageId);
            var editVm = new SchoolUpdateRequest()
            {
                Id = school.Id,
                Code = school.Code,
                Name = school.Name,
                Description = school.Description,
                OrdinalNumber = school.OrdinalNumber,
                Effect = school.Effect,
                DateCreated = school.DateCreated,
                StartDay = school.StartDay,
                EndDay = school.EndDay,
                Note = school.Note,      
            };
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] SchoolUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _schoolApiClient.Update(request);
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
            return PartialView("Delete",new SchoolDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SchoolDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _schoolApiClient.Delete(request.Id);
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
            var result = await _schoolApiClient.GetById(id, languageId);
            return PartialView("Details", result);
        }
    }
}
