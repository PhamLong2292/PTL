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
    public class EthnicsController : ControllerBase
    {
        private readonly IEthnicService _ethnicService;

        public EthnicsController(IEthnicService ethnicService)
        {
            _ethnicService = ethnicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var ethnic = await _ethnicService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(ethnic, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var ethnic = await _ethnicService.GetAllPaging(request);
            return Ok(ethnic);
        }

        [HttpGet("{ethnicId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid ethnicId, string languageId)
        {
            var ethnic = await _ethnicService.GetById(ethnicId, languageId);
            if (ethnic == null)
                return BadRequest("không tìm thấy vùng miền này.");
            return Ok(ethnic);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EthnicCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ethnicService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{ethnicId}")]
        public async Task<IActionResult> Update( [FromBody] EthnicUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ethnicService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{ethnicId}")]
        public async Task<IActionResult> Delete(Guid ethnicId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _ethnicService.Delete(ethnicId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}