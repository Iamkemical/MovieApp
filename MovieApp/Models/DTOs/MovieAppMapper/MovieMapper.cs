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
            CreateMap<SubGenreModel, SubGenreDTO>().ReverseMap();
            CreateMap<SubGenreModel, SubGenreCreateDTO>().ReverseMap();
            CreateMap<SubGenreModel, SubGenreUpdateDTO>().ReverseMap();
            CreateMap<MovieModel, MoviesDTO>().ReverseMap();
            CreateMap<MovieModel, MoviesCreateDTO>().ReverseMap();
            CreateMap<MovieModel, MoviesUpdateDTO>().ReverseMap();
            CreateMap<UserModel, UserAuthDTO>().ReverseMap();
            CreateMap<UserModel, UserRegisterDTO>().ReverseMap();
            CreateMap<UserModel, UserDTO>().ReverseMap();
        }
    }
}
