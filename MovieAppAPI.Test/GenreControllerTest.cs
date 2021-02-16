using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.API.Controllers;
using MovieApp.API.Models;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Models.DTOs.MovieAppMapper;
using MovieApp.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Xunit;

namespace MovieAppAPI.Test
{
    public class GenreControllerTest
    {
        [Fact]
        public void GetAllGenre_NoCondition_Returns_OkObjectResult()
        {
            // Arrange
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var genreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var genreMapper = genreIMapperMock.CreateMapper();
            GenresController genreApiController = new 
                GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var genreDto = new GenreDTO()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid()
            };
            var genreModel = new GenreModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            List<GenreModel> genres = new List<GenreModel>();
            genres.Add(genreModel);
            ICollection<GenreModel> iCollection = genres;
            genreRepositoryMock.Setup(repo => repo.GetGenre())
                .Returns(iCollection);
            // Act
            var genreResult = genreApiController.GetAllGenre();
            var okResult = genreResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void GetGenreById_Returns_OkObjectResult()
        {
            // Arrange
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var genreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var genreMapper = genreIMapperMock.CreateMapper();
            GenresController genreApiController = new 
                GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var genreModel = new GenreModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            genreRepositoryMock.Setup(repo => repo.GetGenre(It.IsAny<Guid>()))
                .Returns(genreModel);
            // Act
            var genreResult = genreApiController.GetGenreById(It.IsAny<Guid>());
            var okResult = genreResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void CreateGenre_Returns_CreatedAtRoute()
        {
            // Arrange
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var genreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var genreMapper = genreIMapperMock.CreateMapper();
            GenresController genreApiController = new 
                GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var genreDto = new GenreDTO()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid()
            };
            genreRepositoryMock.Setup(repo => repo.GenreExist(It.IsAny<string>()))
                .Returns(false);
            genreRepositoryMock.Setup(repo => repo.CreateGenre(It.IsAny<GenreModel>()))
                .Returns(true);

            // Act
            var genreResult = genreApiController.CreateGenre(genreDto);
            var createdAtRouteResult = genreResult as CreatedAtRouteResult;

            // Assert
            Assert.True(createdAtRouteResult.StatusCode is StatusCodes.Status201Created);
        }

        [Fact]
        public void UpdateGenre_Returns_NoContent()
        {
            // Arrange
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var genreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var genreMapper = genreIMapperMock.CreateMapper();
            GenresController genreApiController = new 
                GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var genreDto = new GenreDTO()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid()
            };
            genreRepositoryMock.Setup(repo => repo.UpdateGenre(It.IsAny<GenreModel>()))
                .Returns(true);

            // Act
            var genreResult = genreApiController.UpdateGenre(genreDto.Id, genreDto);
            var noContentResult = genreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void PartialUpdateGenre_Returns_NoContent()
        {
            // Arrange
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var genreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var genreMapper = genreIMapperMock.CreateMapper();
            GenresController genreApiController = new
                GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var patchDoc = new JsonPatchDocument<GenreDTO>();
            var genreDto = new GenreDTO()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            var genreModel = new GenreModel()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            patchDoc.ApplyTo(genreDto, genreApiController.ModelState);
            genreRepositoryMock.Setup(repo => repo.GetGenre(It.IsAny<Guid>()))
                .Returns(genreModel);
            genreRepositoryMock.Setup(repo => repo.UpdateGenre(It.IsAny<GenreModel>()))
                .Returns(true);

            // Act
            var genreResult = genreApiController.PartialUpdateGenre(genreModel.Id, patchDoc);
            var noContentResult = genreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void DeleteGenre_Returns_NoContent()
        {
            // Arrange
            var genreRepositoryMock = new Mock<IGenreRepository>();
            var genreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var genreMapper = genreIMapperMock.CreateMapper();
            GenresController genreApiController = new 
                GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var genreModel = new GenreModel()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid()
            };
            genreRepositoryMock.Setup(repo => repo.GenreExist(It.IsAny<Guid>()))
                .Returns(true);
            genreRepositoryMock.Setup(repo => repo.GetGenre(It.IsAny<Guid>()))
                .Returns(genreModel);
            genreRepositoryMock.Setup(repo => repo.DeleteGenre(It.IsAny<GenreModel>()))
                .Returns(true);

            // Act
            var genreResult = genreApiController.DeleteGenre(genreModel.Id);
            var noContentResult = genreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }
    }
}