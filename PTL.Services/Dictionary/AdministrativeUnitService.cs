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
    public class AdministrativeUnitService : IAdministrativeUnitService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AdministrativeUnitService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }
        public async Task<PagedResult<AdministrativeUnitVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from adu in _context.AdministrativeUnits
                        select new { adu };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.adu.Name.Contains(request.Keyword) || x.adu.Code.Contains(request.Keyword) || x.adu.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new AdministrativeUnitVm()
                {
                    Id = x.adu.Id,
                    Code = x.adu.Code,
                    ShortName = x.adu.ShortName,
                    Name = x.adu.Name,
                    VillageId = x.adu.VillageId,
                    DistrictId = x.adu.DistrictId,
                    ProvinceId = x.adu.ProvinceId,
                    CountryId = x.adu.CountryId,
                    RegionId = x.adu.RegionId,
                    Description = x.adu.Description,
                    OrdinalNumber = x.adu.OrdinalNumber,
                    Effect = x.adu.Effect,
                    DateCreated = x.adu.DateCreated,
                    StartDay = x.adu.StartDay,
                    EndDay = x.adu.EndDay,
                    Note = x.adu.Note,
                    UnsignedName = x.adu.UnsignedName,
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<AdministrativeUnitVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }
        public async Task<ApiResult<PagedResult<AdministrativeUnitVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from adu in _context.AdministrativeUnits
                        select new { adu };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.adu.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AdministrativeUnitVm()
                {
                    Id = x.adu.Id,
                    Code = x.adu.Code,
                    ShortName = x.adu.ShortName,
                    Name = x.adu.Name,
                    VillageId = x.adu.VillageId,
                    DistrictId = x.adu.DistrictId,
                    ProvinceId = x.adu.ProvinceId,
                    CountryId = x.adu.CountryId,
                    RegionId = x.adu.RegionId,
                    Description = x.adu.Description,
                    OrdinalNumber = x.adu.OrdinalNumber,
                    Effect = x.adu.Effect,
                    DateCreated = x.adu.DateCreated,
                    StartDay = x.adu.StartDay,
                    EndDay = x.adu.EndDay,
                    Note = x.adu.Note,
                    UnsignedName = x.adu.UnsignedName,
                }).OrderBy(x => x.OrdinalNumber).ToListAsync();
            int totalRow = await query.CountAsync();
            var pagedResult = new PagedResult<AdministrativeUnitVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<AdministrativeUnitVm>>(pagedResult);
        }
        public async Task<AdministrativeUnitVm> GetById(Guid? AdministrativeUnitId, string languageId)
        {
            var AdministrativeUnit = await _context.AdministrativeUnits.FindAsync(AdministrativeUnitId);    
            if (AdministrativeUnit == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {AdministrativeUnitId}");
            var AdministrativeUnitVm = new AdministrativeUnitVm()
            {
                Id = AdministrativeUnit.Id,
                Code= AdministrativeUnit.Code,
                ShortName = AdministrativeUnit.ShortName,
                Name = AdministrativeUnit.Name,
                VillageId = AdministrativeUnit.VillageId,
                DistrictId = AdministrativeUnit.DistrictId,
                ProvinceId = AdministrativeUnit.ProvinceId,
                CountryId = AdministrativeUnit.CountryId,
                RegionId = AdministrativeUnit.RegionId,
                Description = AdministrativeUnit != null ? AdministrativeUnit.Description : null,
                OrdinalNumber = AdministrativeUnit.OrdinalNumber,
                Effect = AdministrativeUnit.Effect,
                DateCreated = AdministrativeUnit.DateCreated,
                StartDay = AdministrativeUnit.StartDay,
                EndDay= AdministrativeUnit.EndDay,
                Note=AdministrativeUnit.Note, 
                UnsignedName=AdministrativeUnit.UnsignedName,
            };
            return AdministrativeUnitVm;
        }

        public async Task<ApiResult<bool>> Create(AdministrativeUnitCreateRequest request)
        {
            var AdministrativeUnit = await _context.AdministrativeUnits.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (AdministrativeUnit != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.AdministrativeUnits.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.AdministrativeUnit AdministrativeUnits = new PTL.Data.Entities.AdministrativeUnit();
            AdministrativeUnits.Id = Guid.NewGuid();
            AdministrativeUnits.Code = request.Code;
            AdministrativeUnits.ShortName = request.ShortName;
            AdministrativeUnits.Name = request.Name;
            AdministrativeUnits.VillageId = request.VillageId;
            AdministrativeUnits.DistrictId = request.DistrictId;
            AdministrativeUnits.ProvinceId = request.ProvinceId;
            AdministrativeUnits.CountryId = request.CountryId;
            AdministrativeUnits.RegionId = request.RegionId;
            AdministrativeUnits.Description = request.Description;
            AdministrativeUnits.OrdinalNumber = request.OrdinalNumber;
            AdministrativeUnits.Effect = request.Effect;                
            AdministrativeUnits.DateCreated = request.DateCreated; 
            if(AdministrativeUnits.Effect == 1)
            {
                AdministrativeUnits.StartDay = DateTime.Now;
                AdministrativeUnits.EndDay = null;
            }    
            else
            {
                AdministrativeUnits.EndDay = DateTime.Now;
                AdministrativeUnits.StartDay = null;
            }    
            AdministrativeUnits.Note = request.Note;
            AdministrativeUnits.UnsignedName = request.UnsignedName;
            _context.AdministrativeUnits.Add(AdministrativeUnits);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( AdministrativeUnitUpdateRequest request)
        {
            var AdministrativeUnit = await _context.AdministrativeUnits.FindAsync(request.Id);
            if (AdministrativeUnit == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.AdministrativeUnits.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.AdministrativeUnits.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            AdministrativeUnit.Code = request.Code;
            AdministrativeUnit.ShortName = request.ShortName;
            AdministrativeUnit.Name = request.Name;
            AdministrativeUnit.VillageId = request.VillageId;
            AdministrativeUnit.DistrictId = request.DistrictId;
            AdministrativeUnit.ProvinceId = request.ProvinceId;
            AdministrativeUnit.CountryId = request.CountryId;
            AdministrativeUnit.RegionId = request.RegionId;
            AdministrativeUnit.Description = request.Description;
            AdministrativeUnit.OrdinalNumber = request.OrdinalNumber;
            AdministrativeUnit.Effect = request.Effect;
            AdministrativeUnit.DateCreated = request.DateCreated;
            if(AdministrativeUnit.Effect == 1)
            {
                AdministrativeUnit.StartDay = DateTime.Now;
                AdministrativeUnit.EndDay = null;
            }    
            else
            {
                AdministrativeUnit.EndDay = DateTime.Now;
                AdministrativeUnit.StartDay = null;
            }    
            AdministrativeUnit.Note = request.Note;
            AdministrativeUnit.UnsignedName = request.UnsignedName;
            _context.AdministrativeUnits.Update(AdministrativeUnit);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid AdministrativeUnitId)
        {
            var AdministrativeUnit = await _context.AdministrativeUnits.FindAsync(AdministrativeUnitId);
            if (AdministrativeUnit == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.AdministrativeUnits.Remove(AdministrativeUnit);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IAdministrativeUnitService
    {
        Task<PagedResult<AdministrativeUnitVm>> GetSelectAll (GetPagingRequest request);
        Task<ApiResult<PagedResult<AdministrativeUnitVm>>> GetAllPaging(GetPagingRequest request);
        Task<AdministrativeUnitVm> GetById(Guid? AdministrativeUnitId, string languageId);
        Task<ApiResult<bool>> Create(AdministrativeUnitCreateRequest request);
        Task<ApiResult<bool>> Update(AdministrativeUnitUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid AdministrativeUnitId);
    }
}