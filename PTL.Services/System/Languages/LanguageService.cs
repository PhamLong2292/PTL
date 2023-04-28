using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PTL.Data.EF;
using PTL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Services.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public LanguageService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            var langguages = await _context.Languages.Select(x => new LanguageVm()
            {
                Id = x.Id,
                Name = x.Name,
                IsDefault = x.IsDefault,
            }).ToListAsync();
            return new ApiSuccessResult<List<LanguageVm>>(langguages);
        }
    }

    public interface ILanguageService
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}