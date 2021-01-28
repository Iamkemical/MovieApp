using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.API.ApplicationOptions;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.API.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    //[Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthDTO userDto)
        {
            var user = _userRepo.Authenticate(userDto.UserName, userDto.Password);
            if (user is null)
            {
                return BadRequest(new { message = "Username or Password is incorrect!" });
            }

            return Ok(new
            {
                Id = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = user.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(201, Type = typeof(List<UserRegisterDTO>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] UserRegisterDTO model)
        {
            // map dto to entity
             var user = _mapper.Map<UserModel>(model);

            try
            {
                //save
                _userRepo.Create(user, model.Password);
                return CreatedAtRoute("GetUserById", new { userId = user.UserId }, user);
            }
            catch (AppException ex)
            {
                //return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserDTO>))]
        public IActionResult GetAll()
        {
            var users = _userRepo.GetAll();
            var userDto = _mapper.Map<IList<UserDTO>>(users);
            return Ok(userDto);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        [ProducesResponseType(200, Type = typeof(List<UserDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetUserById(int id)
        {
            if (id == null)
            {
                ModelState.AddModelError("", "Check details and try again!");
                return StatusCode(400, ModelState);
            }

            var users = _userRepo.GetUserById(id);
            if(users == null)
            {
                ModelState.AddModelError("", "User does not exist!");
                return StatusCode(404, ModelState);
            }
            var userDto = _mapper.Map<UserDTO>(users);
            return Ok();
        }

        [HttpPut("{id:int}", Name = "Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update(int id, [FromBody] UserRegisterDTO userDto)
        {
            //map to entity and set id
            var user = _mapper.Map<UserModel>(userDto);

            user.UserId = id;

            try
            {
                //save
                _userRepo.Update(user, userDto.Password);
                return NoContent();
            }
            catch (AppException ex)
            {
                //returns error message if there is an exceptiion
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id:int}", Name = "Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(int id)
        {
            _userRepo.Delete(id);
            return NoContent();
        }
    }
}
