using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApp.Web.Models;
using MovieApp.Web.Models.ViewModels;
using MovieApp.Web.Repository.IRepository;

namespace MovieApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenreRepository _genreRepo;
        private readonly ISubGenreRepository _subGenreRepo;
        private readonly IMovieRepository _movieRepo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
            IGenreRepository genreRepo, ISubGenreRepository subGenreRepo,
            IMovieRepository movieRepo)
        {
            _logger = logger;
            _genreRepo = genreRepo;
            _subGenreRepo = subGenreRepo;
            _movieRepo = movieRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listOfGenresSubGenresAndMovies = new IndexVM
            {
                GenreList = await _genreRepo.GetAllAsync(SD.GenreAPIPath),
                SubGenreList = await _subGenreRepo.GetAllAsync(SD.SubGenreAPIPath),
                MovieList = await _movieRepo.GetAllAsync(SD.MovieAPIPath)
            };
            return View(listOfGenresSubGenresAndMovies);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
