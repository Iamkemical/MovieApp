using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
        private readonly IAccountRepository _accountRepository;

        public HomeController(ILogger<HomeController> logger,
            IGenreRepository genreRepo, ISubGenreRepository subGenreRepo,
            IMovieRepository movieRepo, IAccountRepository accountRepository)
        {
            _logger = logger;
            _genreRepo = genreRepo;
            _subGenreRepo = subGenreRepo;
            _movieRepo = movieRepo;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listOfGenresSubGenresAndMovies = new IndexVM
            {
                GenreList = await _genreRepo.GetAllAsync(SD.GenreAPIPath, HttpContext.Session.GetString("JWToken")),
                SubGenreList = await _subGenreRepo.GetAllAsync(SD.SubGenreAPIPath, HttpContext.Session.GetString("JWToken")),
                MovieList = await _movieRepo.GetAllAsync(SD.MovieAPIPath, HttpContext.Session.GetString("JWToken"))
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

        [HttpGet]
        public IActionResult Login()
        {
            UserModel obj = new UserModel();

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserModel obj)
        {
            UserModel objUser = await _accountRepository.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);

            if (objUser.Token is null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


            HttpContext.Session.SetString("JWToken", objUser.Token);
            TempData["alert"] = "Welcome " + objUser.LastName.ToLower() + ", " + objUser.FirstName.ToLower();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel obj)
        {
            bool result = await _accountRepository.RegisterAsync(SD.AccountAPIPath + "register/", obj);

            if (result is false)
            {
                return View();
            }

            TempData["alert"] = "Registeration Successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");

            return RedirectToAction("Index");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
