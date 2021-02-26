using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.API.Models;

namespace MovieApp.API.Repository.IRepository
{
    public interface IGenreRepository
    {
        ICollection<GenreModel> GetGenre();
        GenreModel GetGenre(Guid id);
        bool GenreExist(string name);
        bool GenreExist(Guid id);
        bool CreateGenre(GenreModel model);
        bool UpdateGenre(GenreModel model);
        bool DeleteGenre(GenreModel model);
        bool Save();
    }
}

