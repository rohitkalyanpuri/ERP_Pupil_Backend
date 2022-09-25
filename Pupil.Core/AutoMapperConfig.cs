using AutoMapper;
using Pupil.Core.DataContactsEntities;
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
        }
    }
}
