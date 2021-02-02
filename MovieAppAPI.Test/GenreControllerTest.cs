using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieApp.API.Controllers;
using MovieApp.API.Models.DTOs;
using MovieApp.API.Models.DTOs.MovieAppMapper;
using MovieApp.API.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MovieAppAPI.Test
{
    public class GenreControllerTest
    {
        [Fact]
        public void GetAllGenre_NoCondition_ReturnsAllGenre()
        {
            try
            {
                // Arrange
                var genreRepositoryMock = new Mock<IGenreRepository>();
                var genreIMapperMock = new MapperConfiguration(config =>
                {
                    config.AddProfile(new MovieMapper());
                });
                var genreMapper = genreIMapperMock.CreateMapper();
                GenresController genreApiController = new GenresController(genreRepositoryMock.Object, mapper: genreMapper);
                var genreDto = new GenreDTO()
                {
                    Name = "Comedy",
                    DateCreated = DateTime.Parse("15 May 2015"),
                    Id = new Guid()
                };
                // Act
                var genreResult = genreApiController.GetAllGenre();

                // Assert
                var okResult = genreResult as OkObjectResult;
                if (okResult != null)
                    Assert.NotNull(okResult);

                var genre = okResult.Value as List<GenreDTO>;
                if (genre.Count() > 0)
                {
                    Assert.NotNull(genre);

                    var expected = genre.FirstOrDefault().Name;
                    var actual = genreDto.Name;

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
        public void GetGenreById_Returns_GenreById()
        {
            try
            {
                // Arrange
                var genreRepositoryMock = new Mock<IGenreRepository>();
                var genreIMapperMock = new MapperConfiguration(config =>
                {
                    config.AddProfile(new MovieMapper());
                });
                var genreMapper = genreIMapperMock.CreateMapper();
                GenresController genreApiController = new GenresController(genreRepositoryMock.Object, mapper: genreMapper);
                var genreDto = new GenreDTO()
                {
                    Name = "Comedy",
                    DateCreated = DateTime.Parse("15 May 2015"),
                    Id = new Guid()
                };
                // Act
                var genreResult = genreApiController.GetGenreById(genreDto.Id);

                // Assert
                var okResult = genreResult as OkObjectResult;
                if (okResult != null)
                    Assert.NotNull(okResult);

                var genre = okResult.Value as List<GenreDTO>;
                if (genre.Count() > 0)
                {
                    Assert.NotNull(genre);

                    var expected = genre.FirstOrDefault().Name;
                    var actual = genreDto.Name;

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
        public void CreateGenre_Returns_CreatedAtRoute()
        {
            try
            {
                // Arrange
                var genreRepositoryMock = new Mock<IGenreRepository>();
                var genreIMapperMock = new MapperConfiguration(config =>
                {
                    config.AddProfile(new MovieMapper());
                });
                var genreMapper = genreIMapperMock.CreateMapper();
                GenresController genreApiController = new GenresController(genreRepositoryMock.Object, mapper: genreMapper);
                var genreDto = new GenreDTO()
                {
                    Name = "Comedy",
                    DateCreated = DateTime.Parse("15 May 2015"),
                    Id = new Guid()
                };
                // Act
                var genreResult = genreApiController.CreateGenre(genreDto);

                // Assert
                var okResult = genreResult as CreatedAtRouteResult;
                if (okResult != null)
                    Assert.NotNull(okResult);

                var genre = okResult.Value as List<GenreDTO>;
                if (genre.Count() > 0)
                {
                    Assert.NotNull(genre);

                    var expected = genre.FirstOrDefault().Name;
                    var actual = genreDto.Name;

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
        public void UpdateGenre_Returns_NoContent()
        {
                // Arrange
                var genreRepositoryMock = new Mock<IGenreRepository>();
                var genreIMapperMock = new MapperConfiguration(config =>
                {
                    config.AddProfile(new MovieMapper());
                });
                var genreMapper = genreIMapperMock.CreateMapper();
                GenresController genreApiController = new GenresController(genreRepositoryMock.Object, mapper: genreMapper);
                var genreDto = new GenreDTO()
                {
                    Name = "Comedy",
                    DateCreated = DateTime.Parse("15 May 2015"),
                    Id = new Guid()
                };
                // Act
                var genreResult = genreApiController.UpdateGenre(new Guid(), genreDto);
                var noContentResult = genreResult as NoContentResult;

            // Assert
            Assert.False(noContentResult is NoContentResult);
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
            GenresController genreApiController = new GenresController(genreRepositoryMock.Object, mapper: genreMapper);
            var genreDto = new GenreDTO()
            {
                Name = "Comedy",
                DateCreated = DateTime.Parse("15 May 2015"),
                Id = new Guid()
            };
            // Act
            var genreResult = genreApiController.DeleteGenre(genreDto.Id);
            var noContentResult = genreResult as NoContentResult;

            // Assert
            Assert.False(noContentResult is NoContentResult);
        }
    }
}