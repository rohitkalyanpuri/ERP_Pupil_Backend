using AutoMapper;
using Pupil.Core.DataContactsEntities;
using Pupil.Core.DataTransferObjects;
using Pupil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pupil.Core
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ParentDc,Parent>().ForMember(c => c.Fname,opt => opt.MapFrom(x => x.FirstName))
                .ForMember(c => c.Lname, opt => opt.MapFrom(x => x.LastName));

            CreateMap<StudentDc, Student>().ForMember(c => c.Fname, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(c => c.Lname, opt => opt.MapFrom(x => x.LastName));

            CreateMap<GradeDc, Grade>();

            CreateMap<DivisionDc, Division>().ForMember(c=>c.Ddesc,opt=>opt.MapFrom(x=>x.DivisionDesc))
                .ForMember(c=>c.Dname, opt=>opt.MapFrom(x=>x.DivisionName));
        }
    }
}
