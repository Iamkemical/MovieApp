using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.API.Data;
using MovieApp.API.Models;
using MovieApp.API.Repository.IRepository;

namespace MovieApp.API.Repository
{
    public class SubGenreRepository : ISubGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SubGenreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateSubGenre(SubGenreModel model)
        {
            _dbContext.SubGenres.Add(model);
            return Save();

        }

        public bool DeleteSubGenre(SubGenreModel model)
        {
            _dbContext.SubGenres.Remove(model);
            return Save();
        }

        public bool SubGenreExist(string name)
        {
            bool value = _dbContext.SubGenres.Any(u => u.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool SubGenreExist(Guid id)
        {
            bool value = _dbContext.SubGenres.Any(a => a.Id == id);
            return value;
        }

        public ICollection<SubGenreModel> SubGenre()
        {
            return _dbContext.SubGenres.Include(c => c.Genres).OrderBy(a => a.Name).ToList();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateSubGenre(SubGenreModel model)
        {
            _dbContext.SubGenres.Update(model);
            return Save();
        }

        public SubGenreModel SubGenre(Guid id)
        {
            return _dbContext.SubGenres.Include(c => c.Genres).FirstOrDefault(a => a.Id == id);
        }

        public ICollection<SubGenreModel> GetGenreInSubGenre(Guid genreId)
        {
            return _dbContext.SubGenres.Include(c => c.Genres).Where(c => c.GenreId == genreId).ToList();
        }


        public IEnumerable<SubGenreModel> SubGenreInGenre(Guid Id)
        {
            return _dbContext.SubGenres.ToList()
                .Where(m => m.GenreId == Id);
        }
    }
}
