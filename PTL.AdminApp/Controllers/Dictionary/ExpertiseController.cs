using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;

namespace PTL.AdminApp.Controllers
{
    public class ExpertiseController : Controller
    {
        private readonly IExpertiseApiClient _expertiseApiClient;
        private readonly IConfiguration _configuration;

        public ExpertiseController(IConfiguration configuration, IExpertiseApiClient expertiseApiClient)
        {
            _configuration = configuration;
            _expertiseApiClient = expertiseApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _expertiseApiClient.GetAllPagings(request);
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
            ExpertiseCreateRequest create = new ExpertiseCreateRequest();
            return PartialView("Create", create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExpertiseCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _expertiseApiClient.Create(request);
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
            var expertise = await _expertiseApiClient.GetById(id, languageId);
            var editVm = new ExpertiseUpdateRequest()
            {
                Id = expertise.Id,
                Code = expertise.Code,
                Name = expertise.Name,
                Description = expertise.Description,
                OrdinalNumber = expertise.OrdinalNumber,
                Effect = expertise.Effect,
                DateCreated = expertise.DateCreated,
                StartDay = expertise.StartDay,
                EndDay = expertise.EndDay,
                Note = expertise.Note,      
            };
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] ExpertiseUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _expertiseApiClient.Update(request);
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
            return PartialView("Delete",new ExpertiseDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ExpertiseDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _expertiseApiClient.Delete(request.Id);
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
            var result = await _expertiseApiClient.GetById(id, languageId);
            return PartialView("Details", result);
        }
    }
}
