using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Web
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44371/";
        public static string GenreAPIPath = APIBaseUrl + "api/v1/Genres/";
        public static string SubGenreAPIPath = APIBaseUrl + "api/v1/SubGenres/";
        public static string MovieAPIPath = APIBaseUrl + "api/v1/Movies/";
    }
}
