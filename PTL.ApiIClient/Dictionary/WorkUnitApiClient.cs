using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PTL.Utilities.Constants;
using PTL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PTL.ApiIClient
{
    public class WorkUnitApiClient : BaseApiClient, IWorkUnitApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkUnitApiClient(IHttpClientFactory httpClientFactory,
                    IHttpContextAccessor httpContextAccessor,
                     IConfiguration configuration)
             : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<WorkUnitVm>> GetAll()
        {
            return await GetListAsync<WorkUnitVm>("/api/workunits/pagingall");
        }
        public async Task<PagedResult<WorkUnitVm>> GetSelectAll(GetPagingRequest request)
        {
            var data = await GetAsync<PagedResult<WorkUnitVm>>(
            $"/api/workunits?pageIndex={request.PageIndex}" +
            $"&pageSize={request.PageSize}" +
            $"&keyword={request.Keyword}");
            return data;
        }

        public async Task<ApiResult<PagedResult<WorkUnitVm>>> GetAllPagings(GetPagingRequest request)
        {
            var data = await GetAsync<ApiResult<PagedResult<WorkUnitVm>>>(
            $"/api/workunits/paging?pageIndex={request.PageIndex}" +
            $"&pageSize={request.PageSize}" +
            $"&keyword={request.Keyword}");

            return data;
        }
        public async Task<WorkUnitVm> GetById(Guid? workunitId, string languageId)
        {
            var data = await GetAsync<WorkUnitVm>($"/api/workunits/{workunitId}/{languageId}");

            return data;
        }

        public async Task<ApiResult<bool>> Create(WorkUnitCreateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session .GetString(SystemConstants.AppSettings.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/WorkUnits", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Update(WorkUnitUpdateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/workunits/{request.Id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.DeleteAsync($"/api/workunits/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }
    }

    public interface IWorkUnitApiClient
    {
        Task<List<WorkUnitVm>> GetAll();
        Task<PagedResult<WorkUnitVm>>GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<WorkUnitVm>>> GetAllPagings(GetPagingRequest request);
        Task<WorkUnitVm> GetById(Guid? workunitId, string languageId);
        Task<ApiResult<bool>> Create(WorkUnitCreateRequest request);
        Task<ApiResult<bool>> Update(WorkUnitUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
    }
}