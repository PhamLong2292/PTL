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
    public class DistrictService : IDistrictService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public DistrictService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<PagedResult<DistrictVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from d in _context.Districts
                        select new { d };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.d.Name.Contains(request.Keyword)|| x.d.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new DistrictVm()
                {
                    Id = x.d.Id,
                    Code = x.d.Code,
                    Name = x.d.Name,
                    ProvinceId = x.d.ProvinceId,
                    Description = x.d.Description,
                    OrdinalNumber = x.d.OrdinalNumber,
                    Effect = x.d.Effect,
                    DateCreated = x.d.DateCreated,
                    StartDay = x.d.StartDay,
                    EndDay = x.d.EndDay,
                    Note = x.d.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<DistrictVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<DistrictVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from d in _context.Districts
                        select new { d };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.d.Name.Contains(request.Keyword) || x.d.Code.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DistrictVm()
                {
                    Id = x.d.Id,
                    Code = x.d.Code,
                    Name = x.d.Name,
                    ProvinceId = x.d.ProvinceId,
                    Description = x.d.Description,
                    OrdinalNumber = x.d.OrdinalNumber,
                    Effect = x.d.Effect,
                    DateCreated = x.d.DateCreated,
                    StartDay = x.d.StartDay,
                    EndDay = x.d.EndDay,
                    Note = x.d.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<DistrictVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<DistrictVm>>(pagedResult);
        }
        public async Task<DistrictVm> GetById(Guid? DistrictId, string languageId)
        {
            var District = await _context.Districts.FindAsync(DistrictId);    
            if (District == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {DistrictId}");
            var DistrictVm = new DistrictVm()
            {
                Id = District.Id,
                Code= District.Code,
                Name = District.Name,
                ProvinceId = District.ProvinceId,
                Description = District.Description,
                OrdinalNumber = District.OrdinalNumber,
                Effect = District.Effect,
                DateCreated = District.DateCreated,
                StartDay = District.StartDay,
                EndDay = District.EndDay,
                Note = District.Note,        
            };
            return DistrictVm;
        }

        public async Task<ApiResult<bool>> Create(DistrictCreateRequest request)
        {
            var District = await _context.Districts.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (District != null)
            {
                return new ApiErrorResult<bool>("Mã quốc gia đã tồn tại!");
            }
            if (await _context.Districts.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên quốc gia đã tồn tại");
            }

            PTL.Data.Entities.District districts = new PTL.Data.Entities.District();
            districts.Id = Guid.NewGuid();
            districts.Code = request.Code;
            districts.Name = request.Name;
            districts.ProvinceId = request.ProvinceId;
            districts.ProvinceId = request.ProvinceId;
            districts.Description = request.Description;
            districts.OrdinalNumber = request.OrdinalNumber;
            districts.Effect = request.Effect;
            districts.DateCreated = request.DateCreated; 
            if(districts.Effect == 1)
            {
                districts.StartDay = DateTime.Now;
                districts.EndDay = null;
            }    
            else
            {
                districts.EndDay = DateTime.Now;
                districts.StartDay = null;
            }
            districts.Note = request.Note;
            _context.Districts.Add(districts);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( DistrictUpdateRequest request)
        {
            var districts = await _context.Districts.FindAsync(request.Id);
            if (districts == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Districts.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Districts.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            districts.Code = request.Code;
            districts.Name = request.Name;
            districts.ProvinceId = request.ProvinceId;
            districts.Description = request.Description;
            districts.OrdinalNumber = request.OrdinalNumber;
            districts.Effect = request.Effect;
            districts.DateCreated = request.DateCreated;
            if(districts.Effect == 1)
            {
                districts.StartDay = DateTime.Now;
                districts.EndDay = null;
            }    
            else
            {
                districts.EndDay = DateTime.Now;
                districts.StartDay = null;
            }    
            districts.Note = request.Note;
            _context.Districts.Update(districts);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid DistrictId)
        {
            var District = await _context.Districts.FindAsync(DistrictId);
            if (District == null)
            {
                return new ApiErrorResult<bool>("Quốc gia không tồn tại.");
            }
             _context.Districts.Remove(District);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IDistrictService
    {
        Task<PagedResult<DistrictVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<DistrictVm>>> GetAllPaging(GetPagingRequest request);
        Task<DistrictVm> GetById(Guid? DistrictId, string languageId);
        Task<ApiResult<bool>> Create(DistrictCreateRequest request);
        Task<ApiResult<bool>> Update(DistrictUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid DistrictId);
    }
}