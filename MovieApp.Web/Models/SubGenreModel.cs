﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Web.Models
{
    public class SubGenreModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        public Guid GenreId { get; set; }

        public GenreModel Genres { get; set; }
    }
}
