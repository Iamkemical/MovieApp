using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.API.Models;

namespace MovieApp.API.Repository.IRepository
{
    public interface IMovieRepository
    {
        ICollection<MovieModel> GetMovie();
        ICollection<MovieModel> GetSubGenreInGenre(Guid genreId);
        MovieModel GetMovieById(Guid id);
        bool MovieExist(string name);
        bool MovieExist(Guid id);
        bool CreateMovie(MovieModel model);
        bool UpdateMovie(MovieModel model);
        bool DeleteMovie(MovieModel model);
        bool Save();
    }
}
