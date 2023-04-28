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
    public class ExpertisesController : ControllerBase
    {
        private readonly IExpertiseService _expertiseService;

        public ExpertisesController(IExpertiseService expertiseService)
        {
            _expertiseService = expertiseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var expertise = await _expertiseService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(expertise, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var expertise = await _expertiseService.GetAllPaging(request);
            return Ok(expertise);
        }

        [HttpGet("{expertiseId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid expertiseId, string languageId)
        {
            var expertise = await _expertiseService.GetById(expertiseId, languageId);
            if (expertise == null)
                return BadRequest("không tìm thấy vùng miền này.");
            return Ok(expertise);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpertiseCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _expertiseService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{expertiseId}")]
        public async Task<IActionResult> Update( [FromBody] ExpertiseUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _expertiseService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{expertiseId}")]
        public async Task<IActionResult> Delete(Guid expertiseId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _expertiseService.Delete(expertiseId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}