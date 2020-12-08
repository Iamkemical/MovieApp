using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieApp.Web.Models.ViewModels
{
    public class SubGenreUpsertVM
    {
        public IEnumerable<SelectListItem> GenreList { get; set; }
        public SubGenreModel SubGenre { get; set; }
    }
}
