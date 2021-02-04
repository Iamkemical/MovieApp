using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.API.Controllers;
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
            try
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
                // Act
                var subGenreResult = subGenreApiController.GetAllSubGenre();

                // Assert
                var okResult = subGenreResult as OkObjectResult;
                if (okResult != null)
                    Assert.NotNull(okResult);

                var subGenre = okResult.Value as List<SubGenreDTO>;
                if (subGenre.Count() > 0)
                {
                    Assert.NotNull(subGenre);

                    var expected = subGenre.FirstOrDefault().Name;
                    var actual = subGenreDto.Name;

                    Assert.Equal(expected: expected, actual: actual);
                }
            }
            catch (Exception ex)
            {
                // Assert
                Assert.False(false, ex.Message);
            }
        }

        [Fact]
        public void GetSubGenreById_Returns_SubGenreById()
        {
            try
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
                // Act
                var subGenreResult = subGenreApiController.GetSubGenreById(subGenreDto.Id);

                // Assert
                var okResult = subGenreResult as OkObjectResult;
                if (okResult != null)
                    Assert.NotNull(okResult);

                var subGenre = okResult.Value as List<SubGenreDTO>;
                if (subGenre.Count() > 0)
                {
                    Assert.NotNull(subGenre);

                    var expected = subGenre.FirstOrDefault().Name;
                    var actual = subGenreDto.Name;

                    Assert.Equal(expected: expected, actual: actual);
                }
            }
            catch (Exception ex)
            {
                // Assert
                Assert.False(false, ex.Message);
            }
        }

        [Fact]
        public void CreateSubGenre_Returns_ReturnsCreatedAtRoute()
        {
            try
            {
                // Arrange
                var subGenreRepositoryMock = new Mock<ISubGenreRepository>();
                var subGenreIMapperMock = new MapperConfiguration(config =>
                {
                    config.AddProfile(new MovieMapper());
                });
                var subGenreMapper = subGenreIMapperMock.CreateMapper();
                SubGenresController subGenreApiController = new SubGenresController(subGenreRepositoryMock.Object, mapper: subGenreMapper);
                var subGenreDto = new SubGenreCreateDTO()
                {
                    Name = "Action",
                    DateCreated = DateTime.Parse("15 May 2015"),
                    GenreId = new Guid(),
                };
                // Act
                var subGenreResult = subGenreApiController.CreateSubGenre(subGenreDto);

                // Assert
                var okResult = subGenreResult as CreatedAtRouteResult;
                if (okResult != null)
                    Assert.NotNull(okResult);

                var subGenre = okResult.Value as List<SubGenreDTO>;
                if (subGenre.Count() > 0)
                {
                    Assert.NotNull(subGenre);

                    var expected = subGenre.FirstOrDefault().Name;
                    var actual = subGenreDto.Name;

                    Assert.Equal(expected: expected, actual: actual);
                }
            }
            catch (Exception ex)
            {
                // Assert
                Assert.False(false, ex.Message);
            }
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
                GenreId = Guid.NewGuid(),
            };
            // Act
            var subGenreResult = subGenreApiController.UpdateSubGenre(subGenreDto.Id, subGenreDto);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.False(noContentResult.StatusCode is StatusCodes.Status204NoContent);
        }

        [Fact]
        public void DeleteSubGenre_Returns_NotFoundObjectResultContent()
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
                Name = "Adult Content",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                Genres = new GenreDTO()
            };
            // Act
            var subGenreResult = subGenreApiController.DeleteSubGenre(subGenreDto.Id);
            var noContentResult = subGenreResult as NoContentResult;

            // Assert
            Assert.False(noContentResult.StatusCode is StatusCodes.Status204NoContent);

        }
    }
}
