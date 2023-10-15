using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pupil.DataLayer;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Services
{
    public class ExamTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExamTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ExamType> CreateAsync(string name, string description)
        {
            var type = new ExamType() { Tname = name, Tdesc = description };
            _unitOfWork.ExamRepository.Insert(type);
            await _unitOfWork.SaveChangesAsync();
            return type;
        }



        public async Task<IEnumerable<ExamType>> GetAllAsync()
        {
            return await _unitOfWork.ExamRepository.GetAllAsync();
        }

        public async Task<ExamType> GetByIdAsync(int id)
        {
            return await _unitOfWork.ExamRepository.GetByIdAsync(id);
        }
        public async Task Delete(int id)
        {
            var type = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            _unitOfWork.ExamRepository.Delete(type);
        }
    }
}
