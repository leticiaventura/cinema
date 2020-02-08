using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Cinema.Application.Features.Movies.Commands;
using Cinema.Application.Features.Movies.Queries;
using Cinema.Application.Features.Movies.ViewModels;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.WebAPI.Controllers.Movies;
using Cinema.WebAPI.Tests.Initializer;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Moq;
using NUnit.Framework;

namespace Cinema.WebAPI.Tests.Features.Movies
{
    [TestFixture]
    class MovieControllerTests : TestWepAPIBase
    {
        private MoviesController _moviesController;
        private Mock<IMovieService> _movieServiceMock;

        private Mock<MovieUpdateCommand> _movieUpdateCmd;
        private Mock<MovieAddCommand> _movieAddCmd;
        private Mock<MovieCheckNameQuery> _movieNameQuery;
        private Mock<Movie> _movie;

        private Mock<ValidationResult> _validator;

        [SetUp]
        public void Initialize()
        {
            SetupController();
            SetupCommands();
        }

        #region GET

        [Test]
        public void Movies_Controller_Get_Should_Be_Ok()
        {
            // Arrange
            var id = 1;
            _movie.Setup(x => x.Id).Returns(id);
            var response = new List<Movie>() { _movie.Object }.AsQueryable();
            _movieServiceMock.Setup(s => s.GetAll()).Returns(response);
            var odataOptions = GetOdataQueryOptions<Movie>(_moviesController);

            // Action
            var callback = _moviesController.Get(odataOptions);

            // Assert
            _movieServiceMock.Verify(s => s.GetAll(), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<MovieGridViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Movies_Controller_Should_Get_By_Id_Sucessfully()
        {
            // Arrange
            var id = 1;
            _movie.Setup(a => a.Id).Returns(id);
            _movieServiceMock.Setup(c => c.GetById(id)).Returns(_movie.Object);

            // Action
            IHttpActionResult callback = _moviesController.GetById(id);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<MovieViewModel>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            httpResponse.Content.Id.Should().Be(id);
            _movieServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        #endregion

        #region POST

        [Test]
        public void Movies_Controller_Should_Add_Sucessfully()
        {
            // Arrange
            var id = 1;
            _movie.Setup(l => l.Id).Returns(id);
            _movieServiceMock.Setup(c => c.Add(_movieAddCmd.Object)).Returns(_movie.Object);

            // Action
            IHttpActionResult callback = _moviesController.Post(_movieAddCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<Movie>>().Subject;
            httpResponse.Content.Should().Be(_movie.Object);
            _movieServiceMock.Verify(s => s.Add(_movieAddCmd.Object), Times.Once);
        }

        [Test]
        public void Movies_Controller_Add_Should_Handle_Validation_Errors()
        {
            //Arrange
            var notValid = false;
            _validator.Setup(v => v.IsValid).Returns(notValid);

            // Action
            var callback = _moviesController.Post(_movieAddCmd.Object);

            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _movieAddCmd.Verify(cmd => cmd.Validate(_movieServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _movieAddCmd.VerifyNoOtherCalls();
        }

        [Test]
        public void Movies_Controller_Should_Verify_Available_Name()
        {
            // Arrange
            bool available = true;
            _movieServiceMock.Setup(c => c.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(available);

            // Action
            IHttpActionResult callback = _moviesController.CheckName(_movieNameQuery.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().Be(available);
            _movieServiceMock.Verify(s => s.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>()), Times.Once);
            _movieServiceMock.VerifyNoOtherCalls();
        }

        #endregion

        #region PATCH

        [Test]
        public void Movies_Controller_Should_Update_Sucessfully()
        {
            // Arrange
            var successUpdate = true;
            _movieServiceMock.Setup(c => c.Update(_movieUpdateCmd.Object)).Returns(successUpdate);

            // Action
            IHttpActionResult callback = _moviesController.Update(_movieUpdateCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().BeTrue();
            _movieServiceMock.Verify(s => s.Update(_movieUpdateCmd.Object), Times.Once);
        }

        [Test]
        public void Movies_Controller_Update_Should_Handle_Validation_Errors()
        {
            //Arrange
            var isValid = false;
            _validator.Setup(v => v.IsValid).Returns(isValid);
            // Action
            var callback = _moviesController.Update(_movieUpdateCmd.Object);
            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _movieUpdateCmd.Verify(cmd => cmd.Validate(_movieServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _movieUpdateCmd.VerifyNoOtherCalls();
        }

        #endregion

        #region DELETE

        [Test]
        public void Movies_Controller_Should_Remove_Sucessfully()
        {
            // Arrange
            var isUpdated = true;
            int idToRemove = 1;
            _movieServiceMock.Setup(c => c.Remove(It.IsAny<long>())).Returns(isUpdated);

            // Action
            IHttpActionResult callback = _moviesController.Delete(idToRemove);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            _movieServiceMock.Verify(s => s.Remove(idToRemove), Times.Once);
            httpResponse.Content.Should().BeTrue();
        }

        #endregion

        private void SetupCommands()
        {
            _validator = new Mock<ValidationResult>();
            _movieUpdateCmd = new Mock<MovieUpdateCommand>();
            _movieUpdateCmd.Setup(cmd => cmd.Validate(_movieServiceMock.Object)).Returns(_validator.Object);
            _movieAddCmd = new Mock<MovieAddCommand>();
            _movieAddCmd.Setup(cmd => cmd.Validate(_movieServiceMock.Object)).Returns(_validator.Object);
            _movie = new Mock<Movie>();
            _movieNameQuery = new Mock<MovieCheckNameQuery>();

            var valid = true;
            _validator.Setup(v => v.IsValid).Returns(valid);
        }

        private void SetupController()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            _movieServiceMock = new Mock<IMovieService>();
            _moviesController = new MoviesController(_mapper, _movieServiceMock.Object)
            {
                Request = request,
            };
        }
    }
}
