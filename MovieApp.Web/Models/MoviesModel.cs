using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Web.Models
{
    public class MoviesModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public enum RatingType { Poor, Average, BlockBuster }
        public RatingType Rating { get; set; }
        [Required]
        public Guid GenreId { get; set; }
        public virtual GenreModel Genres { get; set; }
        [Required]
        public Guid SubGenreId { get; set; }
        public virtual SubGenreModel SubGenres { get; set; }
        public enum AudienceType { U, PG, TWELVEA, FIFTEEN, EIGHTEEN }
        public AudienceType Audience { get; set; }
    }
}
