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
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelectAll([FromQuery] GetPagingRequest request)
        {
            var country = await _countryService.GetSelectAll(request);
            string json = JsonConvert.SerializeObject(country, Formatting.Indented);
            return Ok(json);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetPagingRequest request)
        {
            var country = await _countryService.GetAllPaging(request);
            return Ok(country);
        }

        [HttpGet("{countryId}/{languageId}")]
        public async Task<IActionResult> GetById(Guid CountryId, string languageId)
        {
            var country = await _countryService.GetById(CountryId, languageId);
            if (country == null)
                return BadRequest("không tìm thấy quốc gia này.");
            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _countryService.Create(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("{countryId}")]
        public async Task<IActionResult> Update( [FromBody] CountryUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _countryService.Update(request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("{countryId}")]
        public async Task<IActionResult> Delete(Guid countryId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _countryService.Delete(countryId);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}