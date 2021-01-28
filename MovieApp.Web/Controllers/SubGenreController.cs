using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieApp.Web.Models;
using MovieApp.Web.Models.ViewModels;
using MovieApp.Web.Repository.IRepository;

namespace MovieApp.Web.Controllers
{
    [Authorize]
    public class SubGenreController : Controller
    {
        private readonly ISubGenreRepository _subGenreRepo;
        private readonly IGenreRepository _genreRepo;
        public SubGenreController(ISubGenreRepository subGenreRepo, IGenreRepository genreRepo)
        {
            _subGenreRepo = subGenreRepo;
            _genreRepo = genreRepo;
        }
        public IActionResult Index()
        {
            return View(new SubGenreModel());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            IEnumerable<GenreModel> genreList = await _genreRepo.GetAllAsync(SD.GenreAPIPath, HttpContext.Session.GetString("JWToken"));

            SubGenreUpsertVM objVM = new SubGenreUpsertVM()
            {
                GenreList = genreList.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                SubGenre = new SubGenreModel()
            };

            if (id == null)
            {
                // this would be true for insert or create
                return View(objVM);
            }
            // flow will come for update
            objVM.SubGenre = await _subGenreRepo.GetAsync(SD.SubGenreAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM.SubGenre == null)
            {
                return NotFound();
            }
            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(SubGenreUpsertVM objVM)
        {
            if (ModelState.IsValid)
            {
                if (objVM.SubGenre.Id == Guid.Empty)
                {
                    await _subGenreRepo.CreateAsync(SD.SubGenreAPIPath, objVM.SubGenre, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _subGenreRepo.UpdateAsync(SD.SubGenreAPIPath + objVM.SubGenre.Id, objVM.SubGenre, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<GenreModel> genreList = await _genreRepo.GetAllAsync(SD.GenreAPIPath, HttpContext.Session.GetString("JWToken"));

                SubGenreUpsertVM obj = new SubGenreUpsertVM()
                {
                    GenreList = genreList.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    SubGenre = objVM.SubGenre
                };
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllSubGenre()
        {
            return Json(new { data = await _subGenreRepo.GetAllAsync(SD.SubGenreAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var status = await _subGenreRepo.DeleteAsync(SD.SubGenreAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful!" });
            }
            return Json(new { success = false, message = "Delete not Successful!" });
        }

        [ActionName("GetSubGenre")]
        public async Task<IActionResult> GetSubGenre(Guid Id)
        {
            return Json(new { data = await _subGenreRepo.GetAsync(SD.SubGenreAPIPath, Id, HttpContext.Session.GetString("JWToken")) });
        }
    }
}
