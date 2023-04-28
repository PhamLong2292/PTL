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
    public class ExpertiseApiClient : BaseApiClient, IExpertiseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpertiseApiClient(IHttpClientFactory httpClientFactory,
                    IHttpContextAccessor httpContextAccessor,
                     IConfiguration configuration)
             : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ExpertiseVm>> GetAll()
        {
            return await GetListAsync<ExpertiseVm>("/api/expertises/pagingall");
        }
        public async Task<PagedResult<ExpertiseVm>> GetSelectAll(GetPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ExpertiseVm>>(
            $"/api/expertises?pageIndex={request.PageIndex}" +
            $"&pageSize={request.PageSize}" +
            $"&keyword={request.Keyword}");
            return data;
        }

        public async Task<ApiResult<PagedResult<ExpertiseVm>>> GetAllPagings(GetPagingRequest request)
        {
            var data = await GetAsync<ApiResult<PagedResult<ExpertiseVm>>>(
            $"/api/expertises/paging?pageIndex={request.PageIndex}" +
            $"&pageSize={request.PageSize}" +
            $"&keyword={request.Keyword}");

            return data;
        }
        public async Task<ExpertiseVm> GetById(Guid? expertiseId, string languageId)
        {
            var data = await GetAsync<ExpertiseVm>($"/api/expertises/{expertiseId}/{languageId}");

            return data;
        }

        public async Task<ApiResult<bool>> Create(ExpertiseCreateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session .GetString(SystemConstants.AppSettings.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"/api/Expertises", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> Update(ExpertiseUpdateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.Token);

            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/expertises/{request.Id}", httpContent);
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
            var response = await client.DeleteAsync($"/api/expertises/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(body);

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(body);
        }
    }

    public interface IExpertiseApiClient
    {
        Task<List<ExpertiseVm>> GetAll();
        Task<PagedResult<ExpertiseVm>>GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<ExpertiseVm>>> GetAllPagings(GetPagingRequest request);
        Task<ExpertiseVm> GetById(Guid? expertiseId, string languageId);
        Task<ApiResult<bool>> Create(ExpertiseCreateRequest request);
        Task<ApiResult<bool>> Update(ExpertiseUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
    }
}