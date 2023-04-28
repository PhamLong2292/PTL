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
    public class AdministrativeUnitsController : ControllerBase
    {
        private readonly IAdministrativeUnitService _administrativeunitService;

        public AdministrativeUnitsController(IAdministrativeUnitService AdministrativeUnitService)
        {
            _administrativeunitService = AdministrativeUnitService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var administrativeunits = await _administrativeunitService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(administrativeunits, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var administrativeunits = await _administrativeunitService.GetAllPaging(request);
            return Ok(administrativeunits);
        }

        [HttpGet("{administrativeunitId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid administrativeunitId, string languageId)
        {
            var administrativeunits = await _administrativeunitService.GetById(administrativeunitId, languageId);
            if (administrativeunits == null)
                return BadRequest("không tìm thấy đơn vị này.");
            return Ok(administrativeunits);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdministrativeUnitCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _administrativeunitService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{administrativeunitId}")]
        public async Task<IActionResult> Update( [FromBody] AdministrativeUnitUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _administrativeunitService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{administrativeunitId}")]
        public async Task<IActionResult> Delete(Guid administrativeunitId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _administrativeunitService.Delete(administrativeunitId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}