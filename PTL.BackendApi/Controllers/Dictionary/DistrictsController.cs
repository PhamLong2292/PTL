using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PTL.Data.Entities;
using PTL.Services;
using PTL.Services.System.Languages;
using PTL.ViewModels;

namespace PTL.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly IDistrictService _districService;

        public DistrictsController(IDistrictService districtService)
        {
            _districService = districtService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var district = await _districService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(district, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var district = await _districService.GetAllPaging(request);
            return Ok(district);
        }

        [HttpGet("{districtId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid? districtId, string languageId)
        {
            var district = await _districService.GetById(districtId, languageId);
            if (district == null)
                return BadRequest("không tìm thấy quận huyện này.");
            return Ok(district);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DistrictCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _districService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{districtId}")]
        public async Task<IActionResult> Update( [FromBody] DistrictUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _districService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{districtId}")]
        public async Task<IActionResult> Delete(Guid districtId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _districService.Delete(districtId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}