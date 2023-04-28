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
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var region = await _regionService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(region, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var region = await _regionService.GetAllPaging(request);
            return Ok(region);
        }

        [HttpGet("{regionId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid regionId, string languageId)
        {
            var region = await _regionService.GetById(regionId, languageId);
            if (region == null)
                return BadRequest("không tìm thấy vùng miền này.");
            return Ok(region);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegionCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _regionService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{regionId}")]
        public async Task<IActionResult> Update( [FromBody] RegionUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _regionService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{regionId}")]
        public async Task<IActionResult> Delete(Guid regionId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _regionService.Delete(regionId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}