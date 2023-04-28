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
    public class ExpertiseService : IExpertiseService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public ExpertiseService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<PagedResult<ExpertiseVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from r in _context.Expertises
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword) || x.r.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new ExpertiseVm()
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
            var pagedResult = new PagedResult<ExpertiseVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<ExpertiseVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from r in _context.Expertises
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ExpertiseVm()
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
            var pagedResult = new PagedResult<ExpertiseVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<ExpertiseVm>>(pagedResult);
        }
        public async Task<ExpertiseVm> GetById(Guid? expertiseId, string languageId)
        {
            var expertise = await _context.Expertises.FindAsync(expertiseId);    
            if (expertise == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {expertiseId}");
            var expertiseVm = new ExpertiseVm()
            {
                Id = expertise.Id,
                Code= expertise.Code,
                Name = expertise.Name,
                Description = expertise != null ? expertise.Description : null,
                OrdinalNumber = expertise.OrdinalNumber,
                Effect = expertise.Effect,
                DateCreated = expertise.DateCreated,
                StartDay = expertise.StartDay,
                EndDay= expertise.EndDay,
                Note=expertise.Note,        
            };
            return expertiseVm;
        }

        public async Task<ApiResult<bool>> Create(ExpertiseCreateRequest request)
        {
            var expertise = await _context.Expertises.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (expertise != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.Expertises.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.Expertise expertises = new PTL.Data.Entities.Expertise();
            expertises.Id = Guid.NewGuid();
            expertises.Code = request.Code;
            expertises.Name = request.Name;
            expertises.Description = request.Description;
            expertises.OrdinalNumber = request.OrdinalNumber;
            expertises.Effect = request.Effect;                
            expertises.DateCreated = request.DateCreated; 
            if(expertises.Effect == 1)
            {
                expertises.StartDay = DateTime.Now;
                expertises.EndDay = null;
            }    
            else
            {
                expertises.EndDay = DateTime.Now;
                expertises.StartDay = null;
            }    
            expertises.Note = request.Note;
            _context.Expertises.Add(expertises);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( ExpertiseUpdateRequest request)
        {
            var expertise = await _context.Expertises.FindAsync(request.Id);
            if (expertise == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Expertises.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Expertises.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            expertise.Code = request.Code;
            expertise.Name = request.Name;
            expertise.Description = request.Description;
            expertise.OrdinalNumber = request.OrdinalNumber;
            expertise.Effect = request.Effect;
            expertise.DateCreated = request.DateCreated;
            if(expertise.Effect == 1)
            {
                expertise.StartDay = DateTime.Now;
                expertise.EndDay = null;
            }    
            else
            {
                expertise.EndDay = DateTime.Now;
                expertise.StartDay = null;
            }    
            expertise.Note = request.Note;
            _context.Expertises.Update(expertise);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid expertiseId)
        {
            var expertise = await _context.Expertises.FindAsync(expertiseId);
            if (expertise == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.Expertises.Remove(expertise);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IExpertiseService
    {
        Task<PagedResult<ExpertiseVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<ExpertiseVm>>> GetAllPaging(GetPagingRequest request);
        Task<ExpertiseVm> GetById(Guid? expertiseId, string languageId);
        Task<ApiResult<bool>> Create(ExpertiseCreateRequest request);
        Task<ApiResult<bool>> Update(ExpertiseUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid expertiseId);
    }
}