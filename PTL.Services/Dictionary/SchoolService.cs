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
    public class SchoolService : ISchoolService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public SchoolService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<PagedResult<SchoolVm>> GetSelectAll(GetPagingRequest request)
        {
            var query = from r in _context.Schools
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword) || x.r.Code.Contains(request.Keyword));
            }
            var data = await query
                .Select(x => new SchoolVm()
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
            var pagedResult = new PagedResult<SchoolVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<PagedResult<SchoolVm>>> GetAllPaging(GetPagingRequest request)
        {
            var query = from r in _context.Schools
                        select new { r };
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.r.Name.Contains(request.Keyword));
            }
            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new SchoolVm()
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
            var pagedResult = new PagedResult<SchoolVm>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<SchoolVm>>(pagedResult);
        }
        public async Task<SchoolVm> GetById(Guid? schoolId, string languageId)
        {
            var school = await _context.Schools.FindAsync(schoolId);    
            if (school == null)
                 throw new PTLException($"Không tìm thấy vùng miền có Id: {schoolId}");
            var schoolVm = new SchoolVm()
            {
                Id = school.Id,
                Code= school.Code,
                Name = school.Name,
                Description = school != null ? school.Description : null,
                OrdinalNumber = school.OrdinalNumber,
                Effect = school.Effect,
                DateCreated = school.DateCreated,
                StartDay = school.StartDay,
                EndDay= school.EndDay,
                Note=school.Note,        
            };
            return schoolVm;
        }

        public async Task<ApiResult<bool>> Create(SchoolCreateRequest request)
        {
            var school = await _context.Schools.FirstOrDefaultAsync(x => x.Code == request.Code);
            if (school != null)
            {
                return new ApiErrorResult<bool>("Mã vùng đã tồn tại!");
            }
            if (await _context.Schools.FirstOrDefaultAsync (x => x.Name == request.Name) != null)
            {
                return new ApiErrorResult<bool>("Tên vùng đã tồn tại");
            }

            PTL.Data.Entities.School schools = new PTL.Data.Entities.School();
            schools.Id = Guid.NewGuid();
            schools.Code = request.Code;
            schools.Name = request.Name;
            schools.Description = request.Description;
            schools.OrdinalNumber = request.OrdinalNumber;
            schools.Effect = request.Effect;                
            schools.DateCreated = request.DateCreated; 
            if(schools.Effect == 1)
            {
                schools.StartDay = DateTime.Now;
                schools.EndDay = null;
            }    
            else
            {
                schools.EndDay = DateTime.Now;
                schools.StartDay = null;
            }    
            schools.Note = request.Note;
            _context.Schools.Add(schools);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Update( SchoolUpdateRequest request)
        {
            var school = await _context.Schools.FindAsync(request.Id);
            if (school == null) throw new PTLException($"Không tìm thấy id: {request.Id}");
            if (await _context.Schools.AnyAsync(x => x.Code == request.Code && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Mã đã tồn tại");
            }
            if (await _context.Schools.AnyAsync(x => x.Name == request.Name && x.Id != request.Id))
            {
                return new ApiErrorResult<bool>("Tên đã tồn tại");
            }
            school.Code = request.Code;
            school.Name = request.Name;
            school.Description = request.Description;
            school.OrdinalNumber = request.OrdinalNumber;
            school.Effect = request.Effect;
            school.DateCreated = request.DateCreated;
            if(school.Effect == 1)
            {
                school.StartDay = DateTime.Now;
                school.EndDay = null;
            }    
            else
            {
                school.EndDay = DateTime.Now;
                school.StartDay = null;
            }    
            school.Note = request.Note;
            _context.Schools.Update(school);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
        public async Task<ApiResult<bool>> Delete(Guid schoolId)
        {
            var school = await _context.Schools.FindAsync(schoolId);
            if (school == null)
            {
                return new ApiErrorResult<bool>("Vùng miền không tồn tại");
            }
             _context.Schools.Remove(school);
             await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }

    public interface ISchoolService
    {
        Task<PagedResult<SchoolVm>> GetSelectAll(GetPagingRequest request);
        Task<ApiResult<PagedResult<SchoolVm>>> GetAllPaging(GetPagingRequest request);
        Task<SchoolVm> GetById(Guid? schoolId, string languageId);
        Task<ApiResult<bool>> Create(SchoolCreateRequest request);
        Task<ApiResult<bool>> Update(SchoolUpdateRequest request);
        Task<ApiResult<bool>> Delete(Guid schoolId);
    }
}