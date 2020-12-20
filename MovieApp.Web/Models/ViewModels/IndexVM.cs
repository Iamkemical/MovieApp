using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Web.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<GenreModel> GenreList { get; set; }
        public IEnumerable<SubGenreModel> SubGenreList { get; set; }
        public IEnumerable<MoviesModel> MovieList { get; set; }
    }
}
