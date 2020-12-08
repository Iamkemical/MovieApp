using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MovieApp.Web.Models;
using MovieApp.Web.Repository.IRepository;

namespace MovieApp.Web.Repository
{
    public class MovieRepository : Repository<MoviesModel>, IMovieRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        public MovieRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
