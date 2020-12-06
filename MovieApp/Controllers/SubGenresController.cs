using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repository.IRepository;

namespace MovieApp.API.Controllers
{
    [Route("api/v{version:apiVersion}/SubGenres")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class SubGenresController : ControllerBase
    {
        private readonly ISubGenreRepository _genreRepo;
        private readonly IMapper _mapper;

        public SubGenresController(ISubGenreRepository genreRepo,
            IMapper mapper)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of all the Sub genres in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<SubGenreDTO>))]
        public IActionResult GetAllSubGenre()
        {
            var objList = _genreRepo.SubGenre();

            var objDto = new List<SubGenreDTO>();

            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<SubGenreDTO>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Gets individual Sub genre from database
        /// </summary>
        /// <param name="subGenreId">The id of the Sub genre</param>
        /// <returns></returns>
        [HttpGet("{genreId:Guid}", Name = "GetSubGenreById")]
        [ProducesResponseType(200, Type = typeof(List<SubGenreDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetSubGenreById(Guid subGenreId)
        {
            var obj = _genreRepo.SubGenre(subGenreId);

            if (obj is null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<SubGenreDTO>(obj);

            return Ok(objDto);
        }

        /// <summary>
        /// Create a Sub genre
        /// </summary>
        /// <param name="subGenreDto">SubGenre Data transfer object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<SubGenreDTO>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateSubGenre([FromBody] SubGenreCreateDTO subGenreDto)
        {
            if (subGenreDto is null)
            {
                return BadRequest(ModelState);
            }

            if (_genreRepo.SubGenreExist(subGenreDto.Name))
            {
                ModelState.AddModelError("", "SubGenre already exist!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genreObj = _mapper.Map<SubGenreModel>(subGenreDto);

            if (!_genreRepo.CreateSubGenre(genreObj))
            {
                ModelState.AddModelError("", $"Something wrong occured when trying to save record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetSubGenreById", new { genreId = genreObj.Id }, genreObj);
        }
        /// <summary>
        /// Updates existing Sub genre in the database by passing sub genre Id
        /// </summary>
        /// <param name="subGenreId">SubGenre id</param>
        /// <param name="subGenreDto">SubGenre DTO</param>
        /// <returns></returns>
        [HttpPatch("{genreId:Guid}", Name = "UpdateSubGenre")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateSubGenre(Guid subGenreId, [FromBody] SubGenreUpdateDTO subGenreDto)
        {
            if (subGenreDto == null || subGenreId != subGenreDto.Id)
            {
                return BadRequest(ModelState);
            }

            var genreObj = _mapper.Map<SubGenreModel>(subGenreDto);

            if (!_genreRepo.UpdateSubGenre(genreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete Sub genre from database by passing genre id
        /// </summary>
        /// <param name="subGenreId">SubGenre id</param>
        /// <returns></returns>
        [HttpDelete("{genreId:Guid}", Name = "DeleteSubGenre")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteSubGenre(Guid subGenreId)
        {
            if (!_genreRepo.SubGenreExist(subGenreId))
            {
                return NotFound();
            }

            var genreObj = _genreRepo.SubGenre(subGenreId);

            if (!_genreRepo.DeleteSubGenre(genreObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {genreObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
