using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PTL.ApiIClient;
using PTL.Utilities.Constants;
using PTL.ViewModels;

namespace PTL.AdminApp.Controllers
{
    public class EthnicController : Controller
    {
        private readonly IEthnicApiClient _ethnicApiClient;
        private readonly IConfiguration _configuration;

        public EthnicController(IConfiguration configuration, IEthnicApiClient ethnicApiClient)
        {
            _configuration = configuration;
            _ethnicApiClient = ethnicApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _ethnicApiClient.GetAllPagings(request);
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
            EthnicCreateRequest create = new EthnicCreateRequest();
            return PartialView("Create", create);
        }
        [HttpPost]
        public async Task<IActionResult> Create(EthnicCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }

            var result = await _ethnicApiClient.Create(request);
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
            var ethnic = await _ethnicApiClient.GetById(id, languageId);
            var editVm = new EthnicUpdateRequest()
            {
                Id = ethnic.Id,
                Code = ethnic.Code,
                Name = ethnic.Name,
                Description = ethnic.Description,
                OrdinalNumber = ethnic.OrdinalNumber,
                Effect = ethnic.Effect,
                DateCreated = ethnic.DateCreated,
                StartDay = ethnic.StartDay,
                EndDay = ethnic.EndDay,
                Note = ethnic.Note,      
            };
            return PartialView("Edit",editVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EthnicUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                TempData["result"] = "Cập nhật không thành công";
                return RedirectToAction("Index");
            }    
  
            var result = await _ethnicApiClient.Update(request);
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
            return PartialView("Delete",new EthnicDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EthnicDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _ethnicApiClient.Delete(request.Id);
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
            var result = await _ethnicApiClient.GetById(id, languageId);
            return PartialView("Details", result);
        }
    }
}
