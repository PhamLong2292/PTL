using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;

namespace PTL.AdminApp.Controllers
{
    public class WorkUnitController : Controller
    {
        private readonly IWorkUnitApiClient _workunitApiClient;
        private readonly IConfiguration _configuration;

        public WorkUnitController(IConfiguration configuration, IWorkUnitApiClient workunitApiClient)
        {
            _configuration = configuration;
            _workunitApiClient = workunitApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _workunitApiClient.GetAllPagings(request);
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
            WorkUnitCreateRequest create = new WorkUnitCreateRequest();
            return PartialView("Create", create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(WorkUnitCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _workunitApiClient.Create(request);
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
            var workunit = await _workunitApiClient.GetById(id, languageId);
            var editVm = new WorkUnitUpdateRequest()
            {
                Id = workunit.Id,
                Code = workunit.Code,
                Name = workunit.Name,
                Description = workunit.Description,
                OrdinalNumber = workunit.OrdinalNumber,
                Effect = workunit.Effect,
                DateCreated = workunit.DateCreated,
                StartDay = workunit.StartDay,
                EndDay = workunit.EndDay,
                Note = workunit.Note,      
            };
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] WorkUnitUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _workunitApiClient.Update(request);
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
            return PartialView("Delete",new WorkUnitDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(WorkUnitDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _workunitApiClient.Delete(request.Id);
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
            var result = await _workunitApiClient.GetById(id, languageId);
            return PartialView("Details", result);
        }
    }
}
