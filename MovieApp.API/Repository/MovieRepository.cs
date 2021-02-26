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
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MovieRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool CreateMovie(MovieModel model)
        {
            _dbContext.Movies.Add(model);
            return Save();
        }

        public bool DeleteMovie(MovieModel model)
        {
            _dbContext.Movies.Remove(model);
            return Save();
        }

        public ICollection<MovieModel> GetMovie()
        {
            return _dbContext.Movies.Include(a => a.Genres).Include(c => c.SubGenres).OrderBy(u => u.Name).ToList();
        }

        public ICollection<MovieModel> GetGenreInMovie(Guid genreId)
        {
            return _dbContext.Movies.Include(a => a.Genres)
                .Include(c => c.SubGenres)
                .Where(u => u.GenreId == genreId).ToList();
        }

        public bool MovieExist(string name)
        {
            bool value = _dbContext.Movies.Any(u => u.Name == name);
            return value;
        }

        public bool MovieExist(Guid id)
        {
            bool value = _dbContext.Movies.Any(x => x.Id == id);
            return value;
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() >= 0 ? true : false;
        }

        public MovieModel GetMovie(Guid id)
        {
            return _dbContext.Movies.Include(u => u.Genres).Include(b => b.SubGenres).FirstOrDefault(a => a.Id == id);
        }

        public bool UpdateMovie(MovieModel model)
        {
            _dbContext.Movies.Update(model);
            return Save();

        }

        public ICollection<MovieModel> GetSubGenreInMovie(Guid subGenreId)
        {
            return _dbContext.Movies.Include(a => a.Genres)
                .Include(b => b.SubGenres)
                .Where(c => c.SubGenreId == subGenreId).ToList();
        }
    }
}
