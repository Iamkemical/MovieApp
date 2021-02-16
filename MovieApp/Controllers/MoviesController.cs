using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repository.IRepository;

namespace MovieApp.API.Controllers
{
    [Route("api/v{version:apiVersion}/Movies")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IMapper _mapper;
        public MoviesController(IMovieRepository movieRepo, IMapper mapper)
        {
            _movieRepo = movieRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all the Movies in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        public IActionResult GetMovies()
        {
            var objList = _movieRepo.GetMovie();

            var objDto = new List<MoviesDTO>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<MoviesDTO>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Gets individual Movie from database
        /// </summary>
        /// <param name="movieId">The id of the Movie</param>
        /// <returns></returns>
        [HttpGet("{movieId:Guid}", Name = "GetMovieById")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "Admin")]
        public IActionResult GetMovieById(Guid movieId)
        {
            var obj = _movieRepo.GetMovie(movieId);

            if (obj is null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<MoviesDTO>(obj);

            return Ok(objDto);
        }

        /// <summary>
        /// Get list of all genre in the movie
        /// </summary>
        /// <param name="genreId">Id of genre</param>
        /// <returns></returns>
        [HttpGet("[action]/{genreId:Guid}")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetGenreInMovie(Guid genreId)
        {
            var objList = _movieRepo.GetGenreInMovie(genreId);

            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<MoviesDTO>();
            foreach (var obj in objList)
                objDto.Add(_mapper.Map<MoviesDTO>(obj));

            return Ok(objDto);
        }
        /// <summary>
        /// Get list of all subgenre in movie
        /// </summary>
        /// <param name="subGenreId">Id of genre</param>
        /// <returns></returns>
        [HttpGet("[action]/{subGenreId:Guid}")]
        [ProducesResponseType(200, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetSubGenreInMovie(Guid subGenreId)
        {
            var objList = _movieRepo.GetSubGenreInMovie(subGenreId);

            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<MoviesDTO>();
            foreach (var obj in objList)
                objDto.Add(_mapper.Map<MoviesDTO>(obj));

            return Ok(objDto);
        }

        /// <summary>
        /// Create a Movie
        /// </summary>
        /// <param name="moviesDto">Movie Data transfer object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<MoviesDTO>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateMovie([FromBody] MoviesCreateDTO moviesDto)
        {
            if (moviesDto is null)
            {
                return BadRequest(ModelState);
            }

            if (_movieRepo.MovieExist(moviesDto.Name))
            {
                ModelState.AddModelError("", "SubGenre already exist!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieObj = _mapper.Map<MovieModel>(moviesDto);

            if (!_movieRepo.CreateMovie(movieObj))
            {
                ModelState.AddModelError("", $"Something wrong occured when trying to save record {movieObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetMovieById", new { movieId = movieObj.Id }, movieObj);
        }
        /// <summary>
        /// Updates existing movies in the database by passing movie Id
        /// </summary>
        /// <param name="moviesId">Movie id</param>
        /// <param name="moviesDto">Movie DTO</param>
        /// <returns></returns>
        [HttpPut("{moviesId:Guid}", Name = "UpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateMovie(Guid moviesId, [FromBody] MoviesUpdateDTO moviesDto)
        {
            if (moviesDto == null || moviesId != moviesDto.Id)
            {
                return BadRequest(ModelState);
            }

            var genreObj = _mapper.Map<MovieModel>(moviesDto);

            if (!_movieRepo.UpdateMovie(genreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Updates a part of the movie entity in the database
        /// </summary>
        /// <param name="movieId">Id parameter of the movie entity</param>
        /// <param name="patchDoc">Json Patch Document of the movie entity</param>
        /// <returns></returns>
        [HttpPatch("{movieId:Guid}", Name = "PartialUpdateMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PartialUpdateMovie(Guid movieId, JsonPatchDocument<MoviesUpdateDTO> patchDoc)
        {
            var movie = _movieRepo.GetMovie(movieId);
            if (movie == null)
            {
                return NotFound();
            }
            var moviePatchDto = _mapper.Map<MoviesUpdateDTO>(movie);

            patchDoc.ApplyTo(moviePatchDto, ModelState);

            _mapper.Map(moviePatchDto, movie);

            _movieRepo.UpdateMovie(movie);

            return NoContent();
        }

        /// <summary>
        /// Delete Movie from database by passing movie id
        /// </summary>
        /// <param name="moviesId">Movie id</param>
        /// <returns></returns>
        [HttpDelete("{moviesId:Guid}", Name = "DeleteMovie")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteMovie(Guid moviesId)
        {
            if (!_movieRepo.MovieExist(moviesId))
            {
                return NotFound();
            }

            var movieObj = _movieRepo.GetMovie(moviesId);

            if (!_movieRepo.DeleteMovie(movieObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {movieObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
