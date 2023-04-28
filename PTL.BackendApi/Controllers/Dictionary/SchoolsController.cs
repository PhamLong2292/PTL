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
    public class SchoolsController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public SchoolsController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var school = await _schoolService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(school, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var school = await _schoolService.GetAllPaging(request);
            return Ok(school);
        }

        [HttpGet("{schoolId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid schoolId, string languageId)
        {
            var school = await _schoolService.GetById(schoolId, languageId);
            if (school == null)
                return BadRequest("không tìm thấy vùng miền này.");
            return Ok(school);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SchoolCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _schoolService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{schoolId}")]
        public async Task<IActionResult> Update( [FromBody] SchoolUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _schoolService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{schoolId}")]
        public async Task<IActionResult> Delete(Guid schoolId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _schoolService.Delete(schoolId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}