using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.API.Models;

namespace MovieApp.API.Repository.IRepository
{
    public interface ISubGenreRepository
    {
        ICollection<SubGenreModel> SubGenre();
        ICollection<SubGenreModel> GetGenreInSubGenre(Guid genreId);
        IEnumerable<SubGenreModel> SubGenreInGenre(Guid Id);
        SubGenreModel SubGenre(Guid id);
        bool SubGenreExist(string name);
        bool SubGenreExist(Guid id);
        bool CreateSubGenre(SubGenreModel model);
        bool UpdateSubGenre(SubGenreModel model);
        bool DeleteSubGenre(SubGenreModel model);
        bool Save();
    }
}
