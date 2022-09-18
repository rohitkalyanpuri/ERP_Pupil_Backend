using Microsoft.EntityFrameworkCore;
using Pupil.Core.Entities;
using Pupil.Core.Interfaces;
using Pupil.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Infrastructure.Services
{
    public class ExamTypeService : IExamTypeService
    {
        private readonly ApplicationDbContext _context;

        public ExamTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExamType> CreateAsync(string name, string description)
        {
            var type = new ExamType() { Tname=name,Tdesc=description};
            _context.ExamType.Add(type);
            await _context.SaveChangesAsync();
            return type;
        }

        

        public async Task<IReadOnlyList<ExamType>> GetAllAsync()
        {
            return await _context.ExamType.ToListAsync();
        }

        public async Task<ExamType> GetByIdAsync(int id)
        {
            return await _context.ExamType.FindAsync(id);
        }
        public async Task Delete(int id)
        {
            var type = await _context.ExamType.FindAsync(id);
            _context.ExamType.Remove(type);
        }

    }
}
