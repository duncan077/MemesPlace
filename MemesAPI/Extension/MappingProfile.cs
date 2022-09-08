﻿using AutoMapper;
using MemesAPI.Data;
using MemesAPI.Models.User;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MemesAPI.Extension
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MemeUser, UserDTO>()
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))

                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName));
                
            
        }
    }

}