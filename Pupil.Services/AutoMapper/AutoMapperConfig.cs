using AutoMapper;
using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pupil.Services.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ParentDc, Parent>().ForMember(c => c.Fname, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(c => c.Lname, opt => opt.MapFrom(x => x.LastName));

            CreateMap<StudentDc, Student>().ForMember(c => c.Fname, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(c => c.Lname, opt => opt.MapFrom(x => x.LastName));

            CreateMap<GradeDc, Grade>();

            CreateMap<DivisionDc, Division>().ForMember(c => c.Ddesc, opt => opt.MapFrom(x => x.DivisionDesc))
                .ForMember(c => c.Dname, opt => opt.MapFrom(x => x.DivisionName));

            CreateMap<AcademicYearDc, AcademicYear>();
            CreateMap<EnrollmentDc, Enrollment>();
            CreateMap<CourseDc, Course>();
            CreateMap<FeeTypes, FeeTypesDc>();
        }

    }
}
