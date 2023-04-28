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
    public class WorkUnitService : IWorkUnitService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public WorkUnitService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<PagedResult<WorkUnitVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from r in _context.WorkUnits
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword) || x.r.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new WorkUnitVm()
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
            var pagedResult = new PagedResult<WorkUnitVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<WorkUnitVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from r in _context.WorkUnits
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new WorkUnitVm()
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
            var pagedResult = new PagedResult<WorkUnitVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<WorkUnitVm>>(pagedResult);
        }
        public async Task<WorkUnitVm> GetById(Guid? workunitId, string languageId)
        {
            var workunit = await _context.WorkUnits.FindAsync(workunitId);    
            if (workunit == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {workunitId}");
            var workunitVm = new WorkUnitVm()
            {
                Id = workunit.Id,
                Code= workunit.Code,
                Name = workunit.Name,
                Description = workunit != null ? workunit.Description : null,
                OrdinalNumber = workunit.OrdinalNumber,
                Effect = workunit.Effect,
                DateCreated = workunit.DateCreated,
                StartDay = workunit.StartDay,
                EndDay= workunit.EndDay,
                Note=workunit.Note,        
            };
            return workunitVm;
        }

        public async Task<ApiResult<bool>> Create(WorkUnitCreateRequest request)
        {
            var workunit = await _context.WorkUnits.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (workunit != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.WorkUnits.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.WorkUnit workunits = new PTL.Data.Entities.WorkUnit();
            workunits.Id = Guid.NewGuid();
            workunits.Code = request.Code;
            workunits.Name = request.Name;
            workunits.Description = request.Description;
            workunits.OrdinalNumber = request.OrdinalNumber;
            workunits.Effect = request.Effect;                
            workunits.DateCreated = request.DateCreated; 
            if(workunits.Effect == 1)
            {
                workunits.StartDay = DateTime.Now;
                workunits.EndDay = null;
            }    
            else
            {
                workunits.EndDay = DateTime.Now;
                workunits.StartDay = null;
            }    
            workunits.Note = request.Note;
            _context.WorkUnits.Add(workunits);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( WorkUnitUpdateRequest request)
        {
            var workunit = await _context.WorkUnits.FindAsync(request.Id);
            if (workunit == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.WorkUnits.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.WorkUnits.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            workunit.Code = request.Code;
            workunit.Name = request.Name;
            workunit.Description = request.Description;
            workunit.OrdinalNumber = request.OrdinalNumber;
            workunit.Effect = request.Effect;
            workunit.DateCreated = request.DateCreated;
            if(workunit.Effect == 1)
            {
                workunit.StartDay = DateTime.Now;
                workunit.EndDay = null;
            }    
            else
            {
                workunit.EndDay = DateTime.Now;
                workunit.StartDay = null;
            }    
            workunit.Note = request.Note;
            _context.WorkUnits.Update(workunit);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid workunitId)
        {
            var workunit = await _context.WorkUnits.FindAsync(workunitId);
            if (workunit == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.WorkUnits.Remove(workunit);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface IWorkUnitService
    {
        Task<PagedResult<WorkUnitVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<WorkUnitVm>>> GetAllPaging(GetPagingRequest request);
        Task<WorkUnitVm> GetById(Guid? workunitId, string languageId);
        Task<ApiResult<bool>> Create(WorkUnitCreateRequest request);
        Task<ApiResult<bool>> Update(WorkUnitUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid workunitId);
    }
}