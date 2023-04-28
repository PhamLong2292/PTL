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
    public class VillageService : IVillageService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public VillageService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<PagedResult<VillageVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from v in _context.Villages
                        select new { v };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.v.Name.Contains(request.Keyword) || x.v.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new VillageVm()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                    DistrictId = x.v.DistrictId,
                    Description = x.v.Description,
                    OrdinalNumber = x.v.OrdinalNumber,
                    Effect = x.v.Effect,
                    DateCreated = x.v.DateCreated,
                    StartDay = x.v.StartDay,
                    EndDay = x.v.EndDay,
                    Note = x.v.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<VillageVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<VillageVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from v in _context.Villages
                        select new { v };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.v.Name.Contains(request.Keyword) || x.v.Code.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new VillageVm()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                    DistrictId = x.v.DistrictId,
                    Description = x.v.Description,
                    OrdinalNumber = x.v.OrdinalNumber,
                    Effect = x.v.Effect,
                    DateCreated = x.v.DateCreated,
                    StartDay = x.v.StartDay,
                    EndDay = x.v.EndDay,
                    Note = x.v.Note
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<VillageVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<VillageVm>>(pagedResult);
        }
        public async Task<VillageVm> GetById(Guid? VillageId, string languageId)
        {
            var Village = await _context.Villages.FindAsync(VillageId);
            if (Village == null)
                throw new PTLException($"Không tìm thấy vùng miền có Id: {VillageId}");
            var VillageVm = new VillageVm()
            {
                Id = Village.Id,
                Code = Village.Code,
                Name = Village.Name,
                DistrictId = Village.DistrictId,
                Description = Village.Description,
                OrdinalNumber = Village.OrdinalNumber,
                Effect = Village.Effect,
                DateCreated = Village.DateCreated,
                StartDay = Village.StartDay,
                EndDay = Village.EndDay,
                Note = Village.Note,
            };
            return VillageVm;
        }

        public async Task<ApiResult<bool>> Create(VillageCreateRequest request)
        {
            var Village = await _context.Villages.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (Village != null)
            {
                return new ApiErrorResult<bool>("Mã quốc gia đã tồn tại!");
            }
            if (await _context.Villages.FirstOrDefaultAsync(x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên quốc gia đã tồn tại");
            }

            PTL.Data.Entities.Village Villages = new PTL.Data.Entities.Village();
            Villages.Id = Guid.NewGuid();
            Villages.Code = request.Code;
            Villages.Name = request.Name;
            Villages.DistrictId = request.DistrictId;
            Villages.Description = request.Description;
            Villages.OrdinalNumber = request.OrdinalNumber;
            Villages.Effect = request.Effect;
            Villages.DateCreated = request.DateCreated;
            if (Villages.Effect == 1)
            {
                Villages.StartDay = DateTime.Now;
                Villages.EndDay = null;
            }
            else
            {
                Villages.EndDay = DateTime.Now;
                Villages.StartDay = null;
            }
            Villages.Note = request.Note;
            _context.Villages.Add(Villages);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update(VillageUpdateRequest request)
        {
            var Villages = await _context.Villages.FindAsync(request.Id);
            if (Villages == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Villages.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Villages.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            Villages.Code = request.Code;
            Villages.Name = request.Name;
            Villages.DistrictId = request.DistrictId;
            Villages.Description = request.Description;
            Villages.OrdinalNumber = request.OrdinalNumber;
            Villages.Effect = request.Effect;
            Villages.DateCreated = request.DateCreated;
            if (Villages.Effect == 1)
            {
                Villages.StartDay = DateTime.Now;
                Villages.EndDay = null;
            }
            else
            {
                Villages.EndDay = DateTime.Now;
                Villages.StartDay = null;
            }
            Villages.Note = request.Note;
            _context.Villages.Update(Villages);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid VillageId)
        {
            var Village = await _context.Villages.FindAsync(VillageId);
            if (Village == null)
            {
                return new ApiErrorResult<bool>("Quốc gia không tồn tại.");
            }
            _context.Villages.Remove(Village);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IVillageService
    {
        Task<PagedResult<VillageVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<VillageVm>>> GetAllPaging(GetPagingRequest request);
        Task<VillageVm> GetById(Guid? VillageId, string languageId);
        Task<ApiResult<bool>> Create(VillageCreateRequest request);
        Task<ApiResult<bool>> Update(VillageUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid VillageId);
    }
}