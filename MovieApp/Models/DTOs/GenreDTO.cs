using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.API.Models.DTOs
{
    public class GenreDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
