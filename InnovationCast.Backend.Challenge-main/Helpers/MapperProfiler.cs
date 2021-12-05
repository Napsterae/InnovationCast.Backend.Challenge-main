using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Challenge.Entities;
using Backend.Challenge.Dtos;

namespace Backend.Challenge.Helpers
{
    public class MapperProfiler : Profile
    {  
        public MapperProfiler()
        {
            CreateMap<AppComment, CommentResponseDto>().ReverseMap();
            CreateMap<AppComment,AddCommentDto>().ReverseMap();
        }
    }
}