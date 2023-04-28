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
    public class GraduationService : IGraduationService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public GraduationService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<PagedResult<GraduationVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from r in _context.Graduations
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword) || x.r.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new GraduationVm()
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
            var pagedResult = new PagedResult<GraduationVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<GraduationVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from r in _context.Graduations
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GraduationVm()
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
            var pagedResult = new PagedResult<GraduationVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<GraduationVm>>(pagedResult);
        }
        public async Task<GraduationVm> GetById(Guid? graduationId, string languageId)
        {
            var graduation = await _context.Graduations.FindAsync(graduationId);    
            if (graduation == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {graduationId}");
            var graduationVm = new GraduationVm()
            {
                Id = graduation.Id,
                Code= graduation.Code,
                Name = graduation.Name,
                Description = graduation != null ? graduation.Description : null,
                OrdinalNumber = graduation.OrdinalNumber,
                Effect = graduation.Effect,
                DateCreated = graduation.DateCreated,
                StartDay = graduation.StartDay,
                EndDay= graduation.EndDay,
                Note=graduation.Note,        
            };
            return graduationVm;
        }

        public async Task<ApiResult<bool>> Create(GraduationCreateRequest request)
        {
            var graduation = await _context.Graduations.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (graduation != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.Graduations.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.Graduation graduations = new PTL.Data.Entities.Graduation();
            graduations.Id = Guid.NewGuid();
            graduations.Code = request.Code;
            graduations.Name = request.Name;
            graduations.Description = request.Description;
            graduations.OrdinalNumber = request.OrdinalNumber;
            graduations.Effect = request.Effect;                
            graduations.DateCreated = request.DateCreated; 
            if(graduations.Effect == 1)
            {
                graduations.StartDay = DateTime.Now;
                graduations.EndDay = null;
            }    
            else
            {
                graduations.EndDay = DateTime.Now;
                graduations.StartDay = null;
            }    
            graduations.Note = request.Note;
            _context.Graduations.Add(graduations);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( GraduationUpdateRequest request)
        {
            var graduation = await _context.Graduations.FindAsync(request.Id);
            if (graduation == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Graduations.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Graduations.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            graduation.Code = request.Code;
            graduation.Name = request.Name;
            graduation.Description = request.Description;
            graduation.OrdinalNumber = request.OrdinalNumber;
            graduation.Effect = request.Effect;
            graduation.DateCreated = request.DateCreated;
            if(graduation.Effect == 1)
            {
                graduation.StartDay = DateTime.Now;
                graduation.EndDay = null;
            }    
            else
            {
                graduation.EndDay = DateTime.Now;
                graduation.StartDay = null;
            }    
            graduation.Note = request.Note;
            _context.Graduations.Update(graduation);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid graduationId)
        {
            var graduation = await _context.Graduations.FindAsync(graduationId);
            if (graduation == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.Graduations.Remove(graduation);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IGraduationService
    {
        Task<PagedResult<GraduationVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<GraduationVm>>> GetAllPaging(GetPagingRequest request);
        Task<GraduationVm> GetById(Guid? graduationId, string languageId);
        Task<ApiResult<bool>> Create(GraduationCreateRequest request);
        Task<ApiResult<bool>> Update(GraduationUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid graduationId);
    }
}