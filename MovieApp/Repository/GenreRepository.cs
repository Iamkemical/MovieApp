using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieApp.API.Data;
using MovieApp.API.Models;
using MovieApp.API.Repository.IRepository;

namespace MovieApp.API.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GenreRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateGenre(GenreModel model)
        {
            _dbContext.Genres.Add(model);
            return Save();

        }

        public bool DeleteGenre(GenreModel model)
        {
            _dbContext.Genres.Remove(model);
            return Save();
        }

        public bool GenreExist(string name)
        {
            bool value = _dbContext.Genres.Any(u => u.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool GenreExist(Guid id)
        {
            bool value = _dbContext.Genres.Any(a => a.Id == id);
            return value;
        }

        public ICollection<GenreModel> GetGenre()
        {
            return _dbContext.Genres.OrderBy(a => a.Name).ToList();
        }

        public GenreModel GetGenre(Guid id)
        {
            return _dbContext.Genres.FirstOrDefault(a => a.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateGenre(GenreModel model)
        {
            _dbContext.Genres.Update(model);
            return Save();
        }
    }
}
