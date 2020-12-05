using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace MovieApp.API.Models.DTOs.MovieAppMapper
{
    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            CreateMap<GenreModel, GenreDTO>().ReverseMap();
        }
    }
}
