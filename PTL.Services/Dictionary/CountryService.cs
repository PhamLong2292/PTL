using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PTL.Data.EF;
using PTL.Data.Entities;
using PTL.Utilities.Exceptions;
using PTL.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Services
{
    public class CountryService : ICountryService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public CountryService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<List<CountryVm>> GetAll()
        {
            var query = from c in _context.Regions
                        select new { c };
            return await query.Select(x => new CountryVm()
            {
                Id = x.c.Id,
                Code = x.c.Code,
                Name = x.c.Name,
            }).ToListAsync();
        }
        public async Task<PagedResult<CountryVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from c in _context.Countries
                        select new { c };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.c.Name.Contains(request.Keyword)|| x.c.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new CountryVm()
                {
                    Id = x.c.Id,
                    Code = x.c.Code,
                    Name = x.c.Name,
                    Description = x.c.Description,
                    OrdinalNumber = x.c.OrdinalNumber,
                    Effect = x.c.Effect,
                    DateCreated = x.c.DateCreated,
                    StartDay = x.c.StartDay,
                    EndDay = x.c.EndDay,
                    Note = x.c.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<CountryVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<CountryVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from c in _context.Countries
                        select new { c };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.c.Name.Contains(request.Keyword) || x.c.Code.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CountryVm()
                {
                    Id = x.c.Id,
                    Code = x.c.Code,
                    Name = x.c.Name,
                    Description = x.c.Description,
                    OrdinalNumber = x.c.OrdinalNumber,
                    Effect = x.c.Effect,
                    DateCreated = x.c.DateCreated,
                    StartDay = x.c.StartDay,
                    EndDay = x.c.EndDay,
                    Note = x.c.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<CountryVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<CountryVm>>(pagedResult);
        }
        public async Task<CountryVm> GetById(Guid? countryId, string languageId)
        {
            var Country = await _context.Countries.FindAsync(countryId);    
            if (Country == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {countryId}");
            var CountryVm = new CountryVm()
            {
                Id = Country.Id,
                Code= Country.Code,
                Name = Country.Name,
                Description = Country.Description,
                OrdinalNumber = Country.OrdinalNumber,
                Effect = Country.Effect,
                DateCreated = Country.DateCreated,
                StartDay = Country.StartDay,
                EndDay = Country.EndDay,
                Note = Country.Note,        
            };
            return CountryVm;
        }

        public async Task<ApiResult<bool>> Create(CountryCreateRequest request)
        {
            var Country = await _context.Countries.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (Country != null)
            {
                return new ApiErrorResult<bool>("Mã quốc gia đã tồn tại!");
            }
            if (await _context.Countries.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên quốc gia đã tồn tại");
            }

            PTL.Data.Entities.Country Countrys = new PTL.Data.Entities.Country();
            Countrys.Id = Guid.NewGuid();
            Countrys.Code = request.Code;
            Countrys.Name = request.Name;
            Countrys.Description = request.Description;
            Countrys.OrdinalNumber = request.OrdinalNumber;
            Countrys.Effect = request.Effect;                
            Countrys.DateCreated = request.DateCreated; 
            if(Countrys.Effect == 1)
            {
                Countrys.StartDay = DateTime.Now;
                Countrys.EndDay = null;
            }    
            else
            {
                Countrys.EndDay = DateTime.Now;
                Countrys.StartDay = null;
            }    
            Countrys.Note = request.Note;
            _context.Countries.Add(Countrys);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( CountryUpdateRequest request)
        {
            var Country = await _context.Countries.FindAsync(request.Id);
            if (Country == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Countries.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Countries.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            Country.Code = request.Code;
            Country.Name = request.Name;
            Country.Description = request.Description;
            Country.OrdinalNumber = request.OrdinalNumber;
            Country.Effect = request.Effect;
            Country.DateCreated = request.DateCreated;
            if(Country.Effect == 1)
            {
                Country.StartDay = DateTime.Now;
                Country.EndDay = null;
            }    
            else
            {
                Country.EndDay = DateTime.Now;
                Country.StartDay = null;
            }    
            Country.Note = request.Note;
            _context.Countries.Update(Country);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid CountryId)
        {
            var Country = await _context.Countries.FindAsync(CountryId);
            if (Country == null)
            {
                return new ApiErrorResult<bool>("Quốc gia không tồn tại.");
            }
             _context.Countries.Remove(Country);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface ICountryService
    {
        Task<List<CountryVm>> GetAll();
        Task<PagedResult<CountryVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<CountryVm>>> GetAllPaging(GetPagingRequest request);
        Task<CountryVm> GetById(Guid? countryId, string languageId);
        Task<ApiResult<bool>> Create(CountryCreateRequest request);
        Task<ApiResult<bool>> Update(CountryUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid countryId);
    }
}