using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Application.Features.Movies;
using Cinema.Application.Tests.Initializer;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.Domain.Features.Sessions.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cinema.Application.Tests.Features.Movies
{
    [TestFixture]
    class MovieServiceTest : TestServiceBase
    {
        IMovieService _movieService;
        Mock<IMovieRepository> _mockMovieRepository;

        [SetUp]
        public void Initialize()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _movieService = new MovieService( _mockMovieRepository.Object, _mapper);
        }

        #region Add

        [Test]
        public void Movie_Service_Should_Add_Movie_Successfully()
        {
            //Arrange 
            var movie = ObjectMother.GetDefaultMovie();
            var movieCmd = ObjectMother.GetMovieAddCommand();
            _mockMovieRepository.Setup(r => r.Add(It.IsAny<Movie>())).Returns(movie);

            //Action
            var addedMovie = _movieService.Add(movieCmd);

            //Assert
            _mockMovieRepository.Verify(r => r.Add(It.IsAny<Movie>()), Times.Once);
            addedMovie.Id.Should().Be(movie.Id);
        }

        [Test]
        public void Movie_Service_Add_Should_Throw_Exception()
        {
            //Arrange
            var movieCmd = ObjectMother.GetMovieAddCommand();
            _mockMovieRepository.Setup(r => r.Add(It.IsAny<Movie>())).Throws<Exception>();

            //Action
            Action act = () => _movieService.Add(movieCmd);

            //Assert
            act.Should().Throw<Exception>();
            _mockMovieRepository.Verify(r => r.Add(It.IsAny<Movie>()), Times.Once);
        }
        #endregion

        #region Update
        [Test]
        public void Movie_Service_Should_Update_Movie_Sucessfully()
        {
            //Arrange
            var movie = ObjectMother.GetDefaultMovie();
            var movieCmd = ObjectMother.GetMovieUpdateCommand();
            var updated = true;
            _mockMovieRepository.Setup(e => e.GetById(movieCmd.Id)).Returns(movie);
            _mockMovieRepository.Setup(r => r.Save()).Returns(updated);

            //Action
            var updateMovie = _movieService.Update(movieCmd);

            //Assert
            _mockMovieRepository.Verify(e => e.GetById(movieCmd.Id), Times.Once);
            _mockMovieRepository.Verify(r => r.Update(movie), Times.Once);
            updateMovie.Should().BeTrue();
        }

        [Test]
        public void Movie_Service_Update_Should_Not_Run_When_Not_Found()
        {
            //Arrange
            var movieCmd = ObjectMother.GetMovieUpdateCommand();
            _mockMovieRepository.Setup(e => e.GetById(movieCmd.Id)).Returns((Movie)null);

            //Action
            var updateMovie = _movieService.Update(movieCmd);

            //Assert
            _mockMovieRepository.Verify(e => e.GetById(movieCmd.Id), Times.Once);
            _mockMovieRepository.Verify(e => e.Update(It.IsAny<Movie>()), Times.Never);
            updateMovie.Should().BeFalse();
        }

        #endregion

        #region Remove

        [Test]
        public void Movie_Service_Should_Remove_Movie_Sucessfully()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = true;
            _mockMovieRepository.Setup(e => e.Save()).Returns(mockWasRemoved);

            //Action
            var removed = _movieService.Remove(idToRemove);

            //Assert
            _mockMovieRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeTrue();
        }

        [Test]
        public void Movie_Service_Should_Return_False_When_Failed_Remove()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = false;
            _mockMovieRepository.Setup(e => e.Save()).Returns(mockWasRemoved);

            //Action
            var removed = _movieService.Remove(idToRemove);

            //Assert
            _mockMovieRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeFalse();
        }
        #endregion

        #region Get

        [Test]
        public void Movie_Service_Should_Get_Movie_By_Id_Sucessfully()
        {
            //Arrange
            var movie = ObjectMother.GetDefaultMovie();
            _mockMovieRepository.Setup(r => r.GetById(movie.Id)).Returns(movie);

            //Action
            var returnedMovie = _movieService.GetById(movie.Id);

            //Assert
            _mockMovieRepository.Verify(r => r.GetById(movie.Id), Times.Once);
            returnedMovie.Should().NotBeNull();
            returnedMovie.Id.Should().Be(movie.Id);
        }

        [Test]
        public void Movie_Service_Should_Get_All_Movies_Sucessfully()
        {
            //Arrange
            var movie = ObjectMother.GetDefaultMovie();
            var mockValueRepository = new List<Movie>() { movie }.AsQueryable();
            _mockMovieRepository.Setup(r => r.GetAll()).Returns(mockValueRepository);

            //Action
            var movies = _movieService.GetAll();

            //Assert
            _mockMovieRepository.Verify(r => r.GetAll(), Times.Once);
            movies.Should().NotBeNull();
            movies.First().Should().Be(movie);
            movies.Count().Should().Be(mockValueRepository.Count());
        }

        [Test]
        public void Movie_Service_Should_Verify_Movie_Name_Sucessfully()
        {
            //Arrange
            var movie = ObjectMother.GetDefaultMovie();
            _mockMovieRepository.Setup(r => r.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(false);

            //Action
            var result = _movieService.IsNameAlreadyInUse(movie.Name, movie.Id);

            //Assert
            _mockMovieRepository.Verify(r => r.IsNameAlreadyInUse(movie.Name, movie.Id), Times.Once);
            result.Should().BeFalse();
        }
        #endregion
    }
}