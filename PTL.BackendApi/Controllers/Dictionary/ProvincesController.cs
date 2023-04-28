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
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceService _provinceService;

        public ProvincesController(IProvinceService ProvinceService)
        {
            _provinceService = ProvinceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var province = await _provinceService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(province, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var Province = await _provinceService.GetAllPaging(request);
            return Ok(Province);
        }

        [HttpGet("{provinceId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid provinceId, string languageId)
        {
            var Province = await _provinceService.GetById(provinceId, languageId);
            if (Province == null)
                return BadRequest("không tìm thấy tỉnh thành này.");
            return Ok(Province);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProvinceCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _provinceService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{provinceId}")]
        public async Task<IActionResult> Update( [FromBody] ProvinceUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _provinceService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{provinceId}")]
        public async Task<IActionResult> Delete(Guid provinceId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _provinceService.Delete(provinceId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}