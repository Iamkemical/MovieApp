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
using System.Text;
using Xunit;

namespace MovieAppAPI.Test
{
    public class MovieControllerTest
    {
        [Fact]
        public void GetAllSubGenre_NoCondition_Returns_OkResult()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var movieMapper = subGenreIMapperMock.CreateMapper();
            MoviesController movieApiController = new MoviesController(movieRepositoryMock.Object, mapper: movieMapper);
            var movieModel = new MovieModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            List<MovieModel> movies = new List<MovieModel>();
            movies.Add(movieModel);
            ICollection<MovieModel> iCollection = movies;
            movieRepositoryMock.Setup(repo => repo.GetMovie())
                .Returns(iCollection);
            // Act
            var movieResult = movieApiController.GetMovies();
            var okObjectResult = movieResult as OkObjectResult;

            // Assert
            Assert.True(okObjectResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void GetMoviesById_Returns_OkResult()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var movieMapper = subGenreIMapperMock.CreateMapper();
            MoviesController movieApiController = new MoviesController(movieRepositoryMock.Object, mapper: movieMapper);
            var movieModel = new MovieModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            movieRepositoryMock.Setup(repo => repo.GetMovie(It.IsAny<Guid>())).Returns(movieModel);
            // Act
            var movieResult = movieApiController.GetMovieById(Guid.NewGuid());
            var okObjectResult = movieResult as OkObjectResult;

            // Assert
            Assert.True(okObjectResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void CreateMovie_Returns_CreatedAtRouteResult()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var movieMapper = subGenreIMapperMock.CreateMapper();
            MoviesController movieApiController = new MoviesController(movieRepositoryMock.Object, mapper: movieMapper);
            var movieModel = new MovieModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            var movieDto = new MoviesCreateDTO()
            {
                Name = movieModel.Name,
                DateCreated = movieModel.DateCreated,
                GenreId = movieModel.GenreId,
                SubGenreId = Guid.NewGuid()
            };
            movieRepositoryMock.Setup(repo => repo.MovieExist(It.IsAny<string>())).Returns(false);
            movieRepositoryMock.Setup(repo => repo.CreateMovie(It.IsAny<MovieModel>())).Returns(true);
            // Act
            var movieResult = movieApiController.CreateMovie(movieDto);
            var createdAtRouteResult = movieResult as CreatedAtRouteResult;

            // Assert
            Assert.True(createdAtRouteResult.StatusCode is StatusCodes.Status201Created);
        }

        [Fact]
        public void UpdateMovies_Returns_NoContentResult()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var movieMapper = subGenreIMapperMock.CreateMapper();
            MoviesController movieApiController = new MoviesController(movieRepositoryMock.Object, mapper: movieMapper);
            var movieModel = new MovieModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            var movieDto = new MoviesUpdateDTO()
            {
                Id = movieModel.Id,
                Name = movieModel.Name,
                DateCreated = movieModel.DateCreated,
                GenreId = movieModel.GenreId,
                SubGenreId = Guid.NewGuid()
            };
            movieRepositoryMock.Setup(repo => repo.UpdateMovie(It.IsAny<MovieModel>())).Returns(true);
            // Act
            var subGenreResult = movieApiController.UpdateMovie(movieModel.Id, movieDto);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void PartialUpdateMovie_Returns_NoContentResult()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var movieMapper = subGenreIMapperMock.CreateMapper();
            MoviesController movieApiController = new MoviesController(movieRepositoryMock.Object, mapper: movieMapper);
            var patchDoc = new JsonPatchDocument<MoviesUpdateDTO>();
            var movieModel = new MovieModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            var movieDto = new MoviesUpdateDTO()
            {
                Id = movieModel.Id,
                Name = movieModel.Name,
                DateCreated = movieModel.DateCreated,
                GenreId = movieModel.GenreId,
                SubGenreId = Guid.NewGuid()
            };
            movieRepositoryMock.Setup(repo => repo.GetMovie(It.IsAny<Guid>())).Returns(movieModel);
            patchDoc.ApplyTo(movieDto, movieApiController.ModelState);
            movieRepositoryMock.Setup(repo => repo.UpdateMovie(It.IsAny<MovieModel>())).Returns(true);
            // Act
            var subGenreResult = movieApiController.PartialUpdateMovie(movieModel.Id, patchDoc);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void DeleteMovies_Returns_NoContentResult()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var movieMapper = subGenreIMapperMock.CreateMapper();
            MoviesController movieApiController = new MoviesController(movieRepositoryMock.Object, mapper: movieMapper);
            var movieModel = new MovieModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            movieRepositoryMock.Setup(repo => repo.MovieExist(It.IsAny<Guid>())).Returns(true);
            movieRepositoryMock.Setup(repo => repo.GetMovie(It.IsAny<Guid>())).Returns(movieModel);
            movieRepositoryMock.Setup(repo => repo.DeleteMovie(It.IsAny<MovieModel>())).Returns(true);
            // Act
            var subGenreResult = movieApiController.DeleteMovie(movieModel.Id);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }
    }
}
