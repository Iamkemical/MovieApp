using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.API.Controllers;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Models.DTOs.MovieAppMapper;
using MovieApp.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MovieAppAPI.Test
{
    public class UserControllerTest
    {

        [Fact]
        public void GetAllUsers_Returns_OkObjectResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var userMapper = userIMapperMock.CreateMapper();
            UsersController movieApiController = new UsersController(userRepositoryMock.Object, mapper: userMapper);
            var userDto = new UserAuthDTO()
            {
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            int userId = 1;
            var userModel = new UserModel()
            {
                FirstName = "Gabriel",
                LastName = "Isaac",
                Email = "iamkemical1@gmail.com",
                Password = "Pa55word1@",
                UserName = "Iamkemical1",
                UserId = userId
            };

            userRepositoryMock.Setup(repo => repo.GetAll()).Returns(It.IsAny<IEnumerable<UserModel>>());
            // Act
            var userResult = movieApiController.GetAll();
            var okResult = userResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void GetUserById_Returns_OkObjectResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var userMapper = userIMapperMock.CreateMapper();
            UsersController movieApiController = new UsersController(userRepositoryMock.Object, mapper: userMapper);
            var userDto = new UserAuthDTO()
            {
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            int userId = 1;
            var userModel = new UserModel()
            {
                FirstName = "Gabriel",
                LastName = "Isaac",
                Email = "iamkemical1@gmail.com",
                Password = "Pa55word1@",
                UserName = "Iamkemical1",
                UserId = userId
            };

            userRepositoryMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns(userModel);
            // Act
            var userResult = movieApiController.GetUser(userModel.UserId);
            var okResult = userResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void AuthenticateUser_Returns_OkObjectResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var userMapper = userIMapperMock.CreateMapper();
            UsersController userApiController = new UsersController(userRepositoryMock.Object, mapper: userMapper);
            var userDto = new UserAuthDTO()
            {
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            var userModel = new UserModel()
            {
                FirstName = "Gabriel",
                LastName = "Isaac",
                Email = "iamkemical1@gmail.com",
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            userRepositoryMock.Setup(repo => repo.Authenticate(userDto.UserName, userDto.Password)).Returns(userModel);
            // Act
            var userResult = userApiController.Authenticate(userDto);
            var okResult = userResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void RegisterUser_Returns_OkResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var userMapper = userIMapperMock.CreateMapper();
            UsersController userApiController = new UsersController(userRepositoryMock.Object, mapper: userMapper);
            var userDto = new UserRegisterDTO()
            {
                FirstName = "Gabriel",
                LastName = "Isaac",
                Email = "iamkemical1@gmail.com",
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            var userModel = new UserModel()
            {
                FirstName = "Gabriel",
                LastName = "Isaac",
                Email = "iamkemical1@gmail.com",
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            userRepositoryMock.Setup(repo => repo.Create(userModel, userDto.Password));
            // Act
            var userResult = userApiController.Register(userDto);
            var okResult = userResult as OkResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void UpdateUser_Returns_NoContentResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var userMapper = userIMapperMock.CreateMapper();
            UsersController userApiController = new UsersController(userRepositoryMock.Object, mapper: userMapper);
            var userDto = new UserRegisterDTO()
            {
                FirstName = "Gabriel",
                LastName = "Isaac",
                Email = "iamkemical1@gmail.com",
                Password = "Pa55word1@",
                UserName = "Iamkemical1"
            };
            userRepositoryMock.Setup(repo => repo.Update(It.IsAny<UserModel>(), userDto.Password));
            // Act
            var userResult = userApiController.Delete(It.IsAny<int>());
            var noContentResult = userResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void DeleteUser_Returns_NoContentResult()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var userMapper = userIMapperMock.CreateMapper();
            UsersController userApiController = new UsersController(userRepositoryMock.Object, mapper: userMapper);

            userRepositoryMock.Setup(repo => repo.Delete(It.IsAny<int>()));
            // Act
            var userResult = userApiController.Delete(It.IsAny<int>());
            var noContentResult = userResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }
    }
}
