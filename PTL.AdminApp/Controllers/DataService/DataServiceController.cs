using Microsoft.AspNetCore.Mvc;
using PTL.ApiIClient;
using PTL.ViewModels;

namespace PTL.AdminApp.Controllers
{
    public class DataServiceController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAdministrativeUnitApiClient _administrativeUnitApiClient;
        private readonly IVillageApiClient _villageApiClient;
        private readonly IDistrictApiClient _districtApiClient;
        private readonly IProvinceApiClient _provinceApiClient;
        private readonly ICountryApiClient _countryApiClient;
        private readonly IRegionApiClient _regionApiClient;
        private readonly IUserApiClient _userApiClient;

        public DataServiceController(IConfiguration configuration, IAdministrativeUnitApiClient administrativeUnitApiClient, IVillageApiClient villageApiClient, IDistrictApiClient districtApiClient, IProvinceApiClient provinceApiClient, ICountryApiClient countryApiClient, IRegionApiClient regionApiClient, IUserApiClient userApiClient)
        {
            _configuration = configuration;
            _administrativeUnitApiClient = administrativeUnitApiClient;
            _villageApiClient = villageApiClient;
            _districtApiClient = districtApiClient;
            _provinceApiClient = provinceApiClient;
            _countryApiClient = countryApiClient;
            _regionApiClient = regionApiClient;
            _userApiClient = userApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> VillageSelect(string q, int page, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = q,
                PageIndex = page,
                PageSize = pageSize
            };
            var data = await _villageApiClient.GetSelectAll(request);
            return Ok(data);
        }

        public async Task<IActionResult> DistrictSelect(string q, int page, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = q,
                PageIndex = page,
                PageSize = pageSize
            };
            var data = await _districtApiClient.GetSelectAll(request);
            return Ok(data);
        }

        public async Task<IActionResult> ProvinceSelect(string q, int page, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = q,
                PageIndex = page,
                PageSize = pageSize
            };
            var data = await _provinceApiClient.GetSelectAll(request);
            return Ok(data);
        }
        public async Task<IActionResult> CountrySelect(string q, int page, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = q,
                PageIndex = page,
                PageSize = pageSize
            };
            var data = await _countryApiClient.GetSelectAll(request);
            return Ok(data);
        }

        public async Task<IActionResult> RegionSelect(string q, int page, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = q,
                PageIndex = page,
                PageSize = pageSize
            };
            var data = await _regionApiClient.GetSelectAll(request);
            return Ok(data);
        }

        public async Task<IActionResult> UserSelect(string q, int page, int pageSize = 10)
        {
            var request = new GetPagingRequest()
            {
                Keyword = q,
                PageIndex = page,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetSelectAll(request);
            return Ok(data);
        }
    }
}
