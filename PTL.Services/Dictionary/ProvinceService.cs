using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PTL.Data.EF;
using PTL.Data.Entities;
using PTL.Utilities.Exceptions;
using PTL.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTL.Services
{
    public class ProvinceService : IProvinceService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public ProvinceService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<PagedResult<ProvinceVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from p in _context.Provinces
                        select new { p };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword) || x.p.Code.Contains(request.Keyword) || x.p.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new ProvinceVm()
                {
                    Id = x.p.Id,
                    Code = x.p.Code,
                    Name = x.p.Name,
                    CountryId = x.p.CountryId,
                    RegionId = x.p.RegionId,
                    Description = x.p.Description,
                    OrdinalNumber = x.p.OrdinalNumber,
                    Effect = x.p.Effect,
                    DateCreated = x.p.DateCreated,
                    StartDay = x.p.StartDay,
                    EndDay = x.p.EndDay,
                    Note = x.p.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<ProvinceVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }
        public async Task<ApiResult<PagedResult<ProvinceVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from p in _context.Provinces
                        select new {p };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.p.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ProvinceVm()
                {
                    Id = x.p.Id,
                    Code = x.p.Code,
                    Name = x.p.Name,
                    CountryId = x.p.CountryId,
                    RegionId = x.p.RegionId,
                    Description = x.p.Description,
                    OrdinalNumber = x.p.OrdinalNumber,
                    Effect = x.p.Effect,
                    DateCreated = x.p.DateCreated,
                    StartDay = x.p.StartDay,
                    EndDay = x.p.EndDay,
                    Note = x.p.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<ProvinceVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<ProvinceVm>>(pagedResult);
        }
        public async Task<ProvinceVm> GetById(Guid? ProvinceId, string languageId)
        {
            var Province = await _context.Provinces.FindAsync(ProvinceId);    
            if (Province == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {ProvinceId}");
            var ProvinceVm = new ProvinceVm()
            {
                Id = Province.Id,
                Code= Province.Code,
                Name = Province.Name,
                CountryId = Province.CountryId,
                RegionId = Province.RegionId,
                Description = Province != null ? Province.Description : null,
                OrdinalNumber = Province.OrdinalNumber,
                Effect = Province.Effect,
                DateCreated = Province.DateCreated,
                StartDay = Province.StartDay,
                EndDay= Province.EndDay,
                Note=Province.Note,        
            };
            return ProvinceVm;
        }

        public async Task<ApiResult<bool>> Create(ProvinceCreateRequest request)
        {
            var Province = await _context.Provinces.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (Province != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.Provinces.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.Province Provinces = new PTL.Data.Entities.Province();
            Provinces.Id = Guid.NewGuid();
            Provinces.Code = request.Code;
            Provinces.Name = request.Name;
            Provinces.CountryId = request.CountryId;
            Provinces.RegionId = request.RegionId;
            Provinces.Description = request.Description;
            Provinces.OrdinalNumber = request.OrdinalNumber;
            Provinces.Effect = request.Effect;                
            Provinces.DateCreated = request.DateCreated; 
            if(Provinces.Effect == 1)
            {
                Provinces.StartDay = DateTime.Now;
                Provinces.EndDay = null;
            }    
            else
            {
                Provinces.EndDay = DateTime.Now;
                Provinces.StartDay = null;
            }    
            Provinces.Note = request.Note;
            _context.Provinces.Add(Provinces);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( ProvinceUpdateRequest request)
        {
            var Province = await _context.Provinces.FindAsync(request.Id);
            if (Province == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Provinces.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Provinces.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            Province.Code = request.Code;
            Province.Name = request.Name;
            Province.CountryId = request.CountryId;
            Province.RegionId = request.RegionId;
            Province.Description = request.Description;
            Province.OrdinalNumber = request.OrdinalNumber;
            Province.Effect = request.Effect;
            Province.DateCreated = request.DateCreated;
            if(Province.Effect == 1)
            {
                Province.StartDay = DateTime.Now;
                Province.EndDay = null;
            }    
            else
            {
                Province.EndDay = DateTime.Now;
                Province.StartDay = null;
            }    
            Province.Note = request.Note;
            _context.Provinces.Update(Province);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid ProvinceId)
        {
            var Province = await _context.Provinces.FindAsync(ProvinceId);
            if (Province == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.Provinces.Remove(Province);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IProvinceService
    {
        Task<PagedResult<ProvinceVm>> GetSelectAll (GetPagingRequest request);
        Task<ApiResult<PagedResult<ProvinceVm>>> GetAllPaging(GetPagingRequest request);
        Task<ProvinceVm> GetById(Guid? ProvinceId, string languageId);
        Task<ApiResult<bool>> Create(ProvinceCreateRequest request);
        Task<ApiResult<bool>> Update(ProvinceUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid ProvinceId);
    }
}