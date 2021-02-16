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
using System.Text;
using Xunit;

namespace MovieAppAPI.Test
{
    public class SubGenreControllerTest
    {
        [Fact]
        public void GetAllSubGenre_NoCondition_ReturnsAllSubGenre()
        {
            // Arrange
            var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var subGenreMapper = subGenreIMapperMock.CreateMapper();
            SubGenresController subGenreApiController = new SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);
            var subGenreDto = new SubGenreDTO()
            {
                Name = "Action",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid(),
                GenreId = new Guid(),
            };
            var subGenreModel = new SubGenreModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            List<SubGenreModel> subGenres = new List<SubGenreModel>();
            subGenres.Add(subGenreModel);
            ICollection<SubGenreModel> iCollection = subGenres;
            subGenreRepositoryMock.Setup(repo => repo.SubGenre())
                .Returns(iCollection);

            // Act
            var subGenreResult = subGenreApiController.GetAllSubGenre();
            var okResult = subGenreResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void GetSubGenreById_Returns_OK()
        {
            // Arrange
            var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var subGenreMapper = subGenreIMapperMock.CreateMapper();
            SubGenresController subGenreApiController = new SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);
            var subGenreDto = new SubGenreDTO()
            {
                Name = "Action",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid(),
                GenreId = new Guid(),
            };
            var subGenreModel = new SubGenreModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            subGenreRepositoryMock.Setup(repo => repo.SubGenre(It.IsAny<Guid>())).Returns(subGenreModel);
            // Act
            var subGenreResult = subGenreApiController.GetSubGenreById(It.IsAny<Guid>());
            var okResult = subGenreResult as OkObjectResult;

            // Assert
            Assert.True(okResult.StatusCode is StatusCodes.Status200OK);
        }

        [Fact]
        public void CreateSubGenre_Returns_CreatedAtRoute()
        {
            // Arrange
            var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var subGenreMapper = subGenreIMapperMock.CreateMapper();
            SubGenresController subGenreApiController = new SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);
            var genreModel = new GenreModel()
            {
                Name = "Humor",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            var subGenreDto = new SubGenreCreateDTO()
            {
                Name = "Humor",
                DateCreated = DateTime.Parse("15 May 2015"),
                GenreId = genreModel.Id,
                Id = Guid.NewGuid()
            };

            subGenreRepositoryMock.Setup(repo => repo.SubGenreExist(It.IsAny<string>())).Returns(false);
            subGenreRepositoryMock.Setup(repo => repo.CreateSubGenre(It.IsAny<SubGenreModel>())).Returns(true);
            // Act
            var subGenreResult = subGenreApiController.CreateSubGenre(subGenreDto);
            var createdAtRouteResult = subGenreResult as CreatedAtRouteResult;

            // Assert
            Assert.True(createdAtRouteResult.StatusCode is StatusCodes.Status201Created);
        }

        [Fact]
        public void UpdateSubGenre_Returns_NoContent()
        {
            // Arrange
            var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var subGenreMapper = subGenreIMapperMock.CreateMapper();
            SubGenresController subGenreApiController = new SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);

            var subGenreDto = new SubGenreUpdateDTO()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                Genres = new GenreModel()
            };
            subGenreRepositoryMock.Setup(repo => repo.UpdateSubGenre(It.IsAny<SubGenreModel>())).Returns(true);

            // Act
            var subGenreResult = subGenreApiController.UpdateSubGenre(subGenreDto.Id, subGenreDto);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void PartialUpdateSubGenre_Returns_NoContent()
        {
            // Arrange
            var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var subGenreMapper = subGenreIMapperMock.CreateMapper();
            SubGenresController subGenreApiController = new
                SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);
            var patchDoc = new JsonPatchDocument<SubGenreUpdateDTO>();
            var subGenreDto = new SubGenreUpdateDTO()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            var subGenreModel = new SubGenreModel()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid()
            };
            patchDoc.ApplyTo(subGenreDto, subGenreApiController.ModelState);
            subGenreRepositoryMock.Setup(repo => repo.SubGenre(It.IsAny<Guid>()))
                .Returns(subGenreModel);
            subGenreRepositoryMock.Setup(repo => repo.UpdateSubGenre(It.IsAny<SubGenreModel>()))
                .Returns(true);

            // Act
            var subGenreResult = subGenreApiController.PartialUpdateSubGenre(subGenreModel.Id, patchDoc);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void DeleteSubGenre_Returns_NoContentResult()
        {
            // Arrange
            var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
            var subGenreIMapperMock = new MapperConfiguration(config =>
            {
                config.AddProfile(new MovieMapper());
            });
            var subGenreMapper = subGenreIMapperMock.CreateMapper();
            SubGenresController subGenreApiController = new SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);
            var subGenreModel = new SubGenreModel()
            {
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreModel(),
            };
            subGenreRepositoryMock.Setup(repo => repo.SubGenreExist(subGenreModel.Id)).Returns(true);
            subGenreRepositoryMock.Setup(repo => repo.SubGenre(subGenreModel.Id)).Returns(subGenreModel);
            subGenreRepositoryMock.Setup(repo => repo.DeleteSubGenre(subGenreModel)).Returns(true);
            // Act
            var subGenreResult = subGenreApiController.DeleteSubGenre(subGenreModel.Id);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.True(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }
    }
}
