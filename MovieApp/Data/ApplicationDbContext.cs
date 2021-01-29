using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApp.API.Models;

namespace MovieApp.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }

        public DbSet<GenreModel> Genres { get; set; }
        public DbSet<SubGenreModel> SubGenres { get; set; }
        public DbSet<MovieModel> Movies { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
