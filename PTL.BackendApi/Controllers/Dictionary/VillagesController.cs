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
    public class VillagesController : ControllerBase
    {
        private readonly IVillageService _villageService;

        public VillagesController(IVillageService VillageService)
        {
            _villageService = VillageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var Village = await _villageService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(Village, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var Village = await _villageService.GetAllPaging(request);
            return Ok(Village);
        }

        [HttpGet("{villageId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid? villageId, string languageId)
        {
            var Village = await _villageService.GetById(villageId, languageId);
            if (Village == null)
                return BadRequest("không tìm thấy xã phường này.");
            return Ok(Village);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VillageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _villageService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{villageId}")]
        public async Task<IActionResult> Update( [FromBody] VillageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _villageService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{villageId}")]
        public async Task<IActionResult> Delete(Guid VillageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _villageService.Delete(VillageId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}