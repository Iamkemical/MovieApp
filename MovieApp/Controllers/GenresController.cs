using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repository.IRepository;

namespace MovieApp.API.Controllers
{
    [Route("api/v{version:apiVersion}/Genres")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GenresController> _logger;

        public GenresController(IGenreRepository genreRepo, 
            IMapper mapper,
            ILogger<GenresController> logger)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all the Genres in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<GenreDTO>))]
        public IActionResult GetAllGenre()
        {
            var objList = _genreRepo.GetGenre();

            var objDto = new List<GenreDTO>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<GenreDTO>(obj));
            }

            _logger.LogInformation("All genre was successfully retrieved from the database");
            return Ok(objDto);
        }

        /// <summary>
        /// Gets individual genre from database
        /// </summary>
        /// <param name="genreId">The id of the genre</param>
        /// <returns></returns>
        [HttpGet("{genreId:Guid}", Name ="GetGenreById")]
        [ProducesResponseType(200, Type = typeof(List<GenreDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
        public IActionResult GetGenreById(Guid genreId)
        {
            var obj = _genreRepo.GetGenre(genreId);

            if (obj is null)
            {
                _logger.LogError($"There was no resource found for the genre with id: {genreId}");
                return NotFound();
            }

            var objDto = _mapper.Map<GenreDTO>(obj);

            _logger.LogInformation($"Genre with id: {genreId} was successfully retrieved from the database");
            return Ok(objDto);
        }

        /// <summary>
        /// Create a new genre
        /// </summary>
        /// <param name="genreDto">Genre Data transfer object</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<GenreDTO>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateGenre([FromBody] GenreDTO genreDto)
        {
            if (genreDto is null)
            {
                _logger.LogError($"The genreDTO returned a null");
                return BadRequest(ModelState);
            }

            if (_genreRepo.GenreExist(genreDto.Name))
            {
                ModelState.AddModelError("", "Genre already exist!");
                _logger.LogError($"There is already another resource with name, {genreDto.Name} in the database");
                return StatusCode(404, ModelState);            
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("The model state of the DTO is not valid");
                return BadRequest(ModelState);
            }

            var genreObj = _mapper.Map<GenreModel>(genreDto);

            if (!_genreRepo.CreateGenre(genreObj))
            {
                _logger.LogError($"An error occured while trying to create resource with the name: {genreObj.Name}");
                ModelState.AddModelError("", $"Something wrong occured when trying to save record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation($"{genreObj.Name} was successfully created");
            return CreatedAtRoute("GetGenreById", new { genreId = genreObj.Id }, genreObj);
        }
        /// <summary>
        /// Updates existing genre in the database by passing genre Id
        /// </summary>
        /// <param name="genreId">Genre id</param>
        /// <param name="genreDto">Genre DTO</param>
        /// <returns></returns>
        [HttpPut("{genreId:Guid}", Name = "UpdateGenre")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateGenre(Guid genreId, [FromBody]GenreDTO genreDto)
        {
            if (genreDto == null || genreId != genreDto.Id)
            {
                Exception ex = new Exception();
                _logger.LogError("The genreDto is null or the Id in the DTO doesn't correspond with the Genre's Id");
                return BadRequest(ModelState);
            }

            var genreObj = _mapper.Map<GenreModel>(genreDto);

            if (!_genreRepo.UpdateGenre(genreObj))
            {
                _logger.LogError($"An error occured when trying to update the resource in {genreObj.Name}");
                ModelState.AddModelError("", $"Something went wrong when updating the record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation($"{genreObj.Name} was updated successfully");
            return NoContent();
        }
        /// <summary>
        /// Updates only a part of the resource in the database
        /// </summary>
        /// <param name="genreId">Id parameter of the resource to be updated</param>
        /// <param name="patchDoc">Json Patch Document to be updated</param>
        /// <returns></returns>
        [HttpPatch("{genreId:Guid}", Name = "PartialUpdateGenre")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PartialUpdateGenre(Guid genreId, JsonPatchDocument<GenreDTO> patchDoc)
        {
            var genre = _genreRepo.GetGenre(genreId);
            if (genre == null)
            {
                _logger.LogError($"genre with id: {genreId} not available in database");
                return NotFound();
            }
            var genrePatchDto = _mapper.Map<GenreDTO>(genre);

            patchDoc.ApplyTo(genrePatchDto, ModelState);

            _mapper.Map(genrePatchDto, genre);

            _genreRepo.UpdateGenre(genre);

            _logger.LogInformation($"Genre with id: {genreId} was partially updated");

            return NoContent();
        }

        /// <summary>
        /// Delete genre from database by passing genre id
        /// </summary>
        /// <param name="genreId">Genre id</param>
        /// <returns></returns>
        [HttpDelete("{genreId:Guid}", Name = "DeleteGenre")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteGenre(Guid genreId)
        {
            if (!_genreRepo.GenreExist(genreId))
            {
                _logger.LogError($"Genre with id: {genreId} was not found");
                return NotFound();
            }

            var genreObj = _genreRepo.GetGenre(genreId);

            if (!_genreRepo.DeleteGenre(genreObj))
            {
                _logger.LogError($"An error occured when deleting record {genreObj.Name}");
                ModelState.AddModelError("", $"Something went wrong when deleting the record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }

            _logger.LogInformation($"{genreObj.Name} was successfully deleted from the database");
            return NoContent();
        }
    }
}
