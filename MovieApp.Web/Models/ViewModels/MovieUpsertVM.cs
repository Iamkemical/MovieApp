using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieApp.Web.Models.ViewModels
{
    public class MovieUpsertVM
    {
        public IEnumerable<GenreModel> GenreList { get; set; }
        public IEnumerable<SubGenreModel> SubGenreList { get; set; }
        //public IEnumerable<MoviesModel> MovieList { get; set; }
        public MoviesModel Movies { get; set; }
    }
}
