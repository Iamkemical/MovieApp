using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApp.Web.Models;
using MovieApp.Web.Models.ViewModels;
using MovieApp.Web.Repository.IRepository;

namespace MovieApp.Web.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly ISubGenreRepository _subGenreRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly IMovieRepository _movieRepo;

        public MovieController(ISubGenreRepository subGenreRepo,
            IGenreRepository genreRepo,
            IMovieRepository movieRepo)
        {
            _subGenreRepo = subGenreRepo;
            _genreRepo = genreRepo;
            _movieRepo = movieRepo;
        }
        public IActionResult Index()
        {
            return View(new MoviesModel());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(Guid? id)
        {

            MovieUpsertVM obj = new MovieUpsertVM()
            {
                GenreList = await _genreRepo.GetAllAsync(SD.GenreAPIPath),
                SubGenreList = await _subGenreRepo.GetAllAsync(SD.SubGenreAPIPath),
                Movies = new MoviesModel()
            };


            if (id == null)
            {
                // this would be true for insert or create
                return View(obj);
            }
            // flow will come for update
            obj.Movies = await _movieRepo.GetAsync(SD.MovieAPIPath, id.GetValueOrDefault());
            if (obj.Movies == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(MovieUpsertVM objVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    objVM.Movies.Picture = p1;
                }
                else
                {
                    var objFromDb = await _movieRepo.GetAsync(SD.MovieAPIPath, objVM.Movies.Id);
                    objVM.Movies.Picture = objFromDb.Picture;
                }
                if (objVM.Movies.Id == Guid.Empty)
                {
                    await _movieRepo.CreateAsync(SD.MovieAPIPath, objVM.Movies);
                }
                else
                {
                    await _movieRepo.UpdateAsync(SD.MovieAPIPath + objVM.Movies.Id, objVM.Movies);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                MovieUpsertVM obj = new MovieUpsertVM()
                {
                    GenreList = await _genreRepo.GetAllAsync(SD.GenreAPIPath),
                    SubGenreList = await _subGenreRepo.GetAllAsync(SD.SubGenreAPIPath),
                    Movies = objVM.Movies
                };
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllMovies()
        {
            return Json(new { data = await _movieRepo.GetAllAsync(SD.MovieAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _movieRepo.DeleteAsync(SD.MovieAPIPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful!" });
            }
            return Json(new { success = false, message = "Delete not Successful!" });
        }
    }
}
