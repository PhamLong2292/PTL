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
    public class GraduationsController : ControllerBase
    {
        private readonly IGraduationService _graduationService;

        public GraduationsController(IGraduationService graduationService)
        {
            _graduationService = graduationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var graduation = await _graduationService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(graduation, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var graduation = await _graduationService.GetAllPaging(request);
            return Ok(graduation);
        }

        [HttpGet("{graduationId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid graduationId, string languageId)
        {
            var graduation = await _graduationService.GetById(graduationId, languageId);
            if (graduation == null)
                return BadRequest("không tìm thấy vùng miền này.");
            return Ok(graduation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GraduationCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _graduationService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{graduationId}")]
        public async Task<IActionResult> Update( [FromBody] GraduationUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _graduationService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{graduationId}")]
        public async Task<IActionResult> Delete(Guid graduationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _graduationService.Delete(graduationId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}