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
    public class RegionService : IRegionService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public RegionService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<PagedResult<RegionVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from r in _context.Regions
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword) || x.r.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new RegionVm()
                {
                    Id = x.r.Id,
                    Code = x.r.Code,
                    Name = x.r.Name,
                    Description = x.r.Description,
                    OrdinalNumber = x.r.OrdinalNumber,
                    Effect = x.r.Effect,
                    DateCreated = x.r.DateCreated,
                    StartDay = x.r.StartDay,
                    EndDay = x.r.EndDay,
                    Note = x.r.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<RegionVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<RegionVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from r in _context.Regions
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new RegionVm()
                {
                    Id = x.r.Id,
                    Code = x.r.Code,
                    Name = x.r.Name,
                    Description = x.r.Description,
                    OrdinalNumber = x.r.OrdinalNumber,
                    Effect = x.r.Effect,
                    DateCreated = x.r.DateCreated,
                    StartDay = x.r.StartDay,
                    EndDay = x.r.EndDay,
                    Note = x.r.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<RegionVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<RegionVm>>(pagedResult);
        }
        public async Task<RegionVm> GetById(Guid? regionId, string languageId)
        {
            var region = await _context.Regions.FindAsync(regionId);    
            if (region == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {regionId}");
            var regionVm = new RegionVm()
            {
                Id = region.Id,
                Code= region.Code,
                Name = region.Name,
                Description = region != null ? region.Description : null,
                OrdinalNumber = region.OrdinalNumber,
                Effect = region.Effect,
                DateCreated = region.DateCreated,
                StartDay = region.StartDay,
                EndDay= region.EndDay,
                Note=region.Note,        
            };
            return regionVm;
        }

        public async Task<ApiResult<bool>> Create(RegionCreateRequest request)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (region != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.Regions.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.Region regions = new PTL.Data.Entities.Region();
            regions.Id = Guid.NewGuid();
            regions.Code = request.Code;
            regions.Name = request.Name;
            regions.Description = request.Description;
            regions.OrdinalNumber = request.OrdinalNumber;
            regions.Effect = request.Effect;                
            regions.DateCreated = request.DateCreated; 
            if(regions.Effect == 1)
            {
                regions.StartDay = DateTime.Now;
                regions.EndDay = null;
            }    
            else
            {
                regions.EndDay = DateTime.Now;
                regions.StartDay = null;
            }    
            regions.Note = request.Note;
            _context.Regions.Add(regions);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( RegionUpdateRequest request)
        {
            var region = await _context.Regions.FindAsync(request.Id);
            if (region == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Regions.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Regions.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            region.Code = request.Code;
            region.Name = request.Name;
            region.Description = request.Description;
            region.OrdinalNumber = request.OrdinalNumber;
            region.Effect = request.Effect;
            region.DateCreated = request.DateCreated;
            if(region.Effect == 1)
            {
                region.StartDay = DateTime.Now;
                region.EndDay = null;
            }    
            else
            {
                region.EndDay = DateTime.Now;
                region.StartDay = null;
            }    
            region.Note = request.Note;
            _context.Regions.Update(region);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid regionId)
        {
            var region = await _context.Regions.FindAsync(regionId);
            if (region == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.Regions.Remove(region);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IRegionService
    {
        Task<PagedResult<RegionVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<RegionVm>>> GetAllPaging(GetPagingRequest request);
        Task<RegionVm> GetById(Guid? regionId, string languageId);
        Task<ApiResult<bool>> Create(RegionCreateRequest request);
        Task<ApiResult<bool>> Update(RegionUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid regionId);
    }
}