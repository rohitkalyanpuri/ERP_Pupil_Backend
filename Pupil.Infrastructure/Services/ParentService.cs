using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.Entities;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Infrastructure.Services
{
    public class ParentService : IParentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ParentService(ApplicationDbContext context,IMapper mapper)
        {
            _context = context; _mapper = mapper;
        }

        public async Task<ParentDc> CreateAsync(ParentDc parentDc)
        {
            var tblParent = _mapper.Map<Parent>(parentDc);
            _context.Parent.Add(tblParent);
            await _context.SaveChangesAsync();
            parentDc.ParentId = tblParent.ParentId;
            return parentDc;
        }



        public IEnumerable<ParentDc> GetAllSync()
        {
            IEnumerable<ParentDc> parents = _context.Parent.Select(x => new ParentDc
            {
                FirstName = x.Fname,
                LastName = x.Lname,
                Email = x.Email,
                Phone = x.Phone,
                Mobile = x.Mobile,
                LastLoginDate=x.LastLoginDate,
                LastLoginIp=x.LastLoginIp,
                DOB=x.DOB,
                Status=x.Status,
                Password=x.Password,
                ParentId=x.ParentId
            });
            return parents;
        }

        public async Task<ExamType> GetByIdAsync(int id)
        {
            return await _context.ExamType.FindAsync(id);
        }
        public async Task Delete(int id)
        {
            var parent = await _context.Parent.FindAsync(id);
            _context.Parent.Remove(parent);
            _context.SaveChanges();
        }

        public async Task<ParentDc> UpdateAsync(ParentDc parentDc)
        {
            var parent = await _context.Parent.FindAsync(parentDc.ParentId);
            parent.Fname = parentDc.FirstName;
            parent.Lname = parentDc.LastName;
            parent.Email = parentDc.Email;
            parent.Mobile = parentDc.Mobile;
            parent.DOB = parentDc.DOB;
            _context.Parent.Update(parent);
            _context.SaveChanges();
            return parentDc;
        }
    }
}
