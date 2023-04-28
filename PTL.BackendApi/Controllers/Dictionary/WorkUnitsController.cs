using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PTL.Services;
using PTL.Services.System.Languages;
using PTL.ViewModels;

namespace PTL.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkUnitsController : ControllerBase
    {
        private readonly IWorkUnitService _workunitService;

        public WorkUnitsController(IWorkUnitService workunitService)
        {
            _workunitService = workunitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var workunit = await _workunitService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(workunit, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var workunit = await _workunitService.GetAllPaging(request);
            return Ok(workunit);
        }

        [HttpGet("{workunitId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid workunitId, string languageId)
        {
            var workunit = await _workunitService.GetById(workunitId, languageId);
            if (workunit == null)
                return BadRequest("không tìm thấy vùng miền này.");
            return Ok(workunit);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkUnitCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workunitService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{workunitId}")]
        public async Task<IActionResult> Update( [FromBody] WorkUnitUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workunitService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{workunitId}")]
        public async Task<IActionResult> Delete(Guid workunitId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _workunitService.Delete(workunitId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}