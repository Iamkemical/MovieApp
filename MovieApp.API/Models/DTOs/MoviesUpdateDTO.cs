using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static MovieApp.API.Models.MovieModel;

namespace MovieApp.API.Models.DTOs
{
    public class MoviesUpdateDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte[] Picture { get; set; }
        public RatingType Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public AudienceType Audience { get; set; }
        [Required]
        public Guid GenreId { get; set; }
        [Required]
        public Guid SubGenreId { get; set; }
    }
}
