using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.API.Models
{
    public class SubGenreModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        [Required]
        public Guid GenreId { get; set; }

        [ForeignKey("GenreId")]
        public GenreModel Genres { get; set; }


    }
}
