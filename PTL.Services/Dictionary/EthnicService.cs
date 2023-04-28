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
    public class EthnicService : IEthnicService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public EthnicService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<PagedResult<EthnicVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from r in _context.Ethnics
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword) || x.r.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new EthnicVm()
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
            var pagedResult = new PagedResult<EthnicVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<EthnicVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from r in _context.Ethnics
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new EthnicVm()
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
            var pagedResult = new PagedResult<EthnicVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<EthnicVm>>(pagedResult);
        }
        public async Task<EthnicVm> GetById(Guid? ethnicId, string languageId)
        {
            var ethnic = await _context.Ethnics.FindAsync(ethnicId);    
            if (ethnic == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {ethnicId}");
            var ethnicVm = new EthnicVm()
            {
                Id = ethnic.Id,
                Code= ethnic.Code,
                Name = ethnic.Name,
                Description = ethnic != null ? ethnic.Description : null,
                OrdinalNumber = ethnic.OrdinalNumber,
                Effect = ethnic.Effect,
                DateCreated = ethnic.DateCreated,
                StartDay = ethnic.StartDay,
                EndDay= ethnic.EndDay,
                Note=ethnic.Note,        
            };
            return ethnicVm;
        }

        public async Task<ApiResult<bool>> Create(EthnicCreateRequest request)
        {
            var ethnic = await _context.Ethnics.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (ethnic != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.Ethnics.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.Ethnic ethnics = new PTL.Data.Entities.Ethnic();
            ethnics.Id = Guid.NewGuid();
            ethnics.Code = request.Code;
            ethnics.Name = request.Name;
            ethnics.Description = request.Description;
            ethnics.OrdinalNumber = request.OrdinalNumber;
            ethnics.Effect = request.Effect;                
            ethnics.DateCreated = request.DateCreated; 
            if(ethnics.Effect == 1)
            {
                ethnics.StartDay = DateTime.Now;
                ethnics.EndDay = null;
            }    
            else
            {
                ethnics.EndDay = DateTime.Now;
                ethnics.StartDay = null;
            }    
            ethnics.Note = request.Note;
            _context.Ethnics.Add(ethnics);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( EthnicUpdateRequest request)
        {
            var ethnic = await _context.Ethnics.FindAsync(request.Id);
            if (ethnic == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Ethnics.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Ethnics.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            ethnic.Code = request.Code;
            ethnic.Name = request.Name;
            ethnic.Description = request.Description;
            ethnic.OrdinalNumber = request.OrdinalNumber;
            ethnic.Effect = request.Effect;
            ethnic.DateCreated = request.DateCreated;
            if(ethnic.Effect == 1)
            {
                ethnic.StartDay = DateTime.Now;
                ethnic.EndDay = null;
            }    
            else
            {
                ethnic.EndDay = DateTime.Now;
                ethnic.StartDay = null;
            }    
            ethnic.Note = request.Note;
            _context.Ethnics.Update(ethnic);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid ethnicId)
        {
            var ethnic = await _context.Ethnics.FindAsync(ethnicId);
            if (ethnic == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.Ethnics.Remove(ethnic);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IEthnicService
    {
        Task<PagedResult<EthnicVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<EthnicVm>>> GetAllPaging(GetPagingRequest request);
        Task<EthnicVm> GetById(Guid? ethnicId, string languageId);
        Task<ApiResult<bool>> Create(EthnicCreateRequest request);
        Task<ApiResult<bool>> Update(EthnicUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid ethnicId);
    }
}