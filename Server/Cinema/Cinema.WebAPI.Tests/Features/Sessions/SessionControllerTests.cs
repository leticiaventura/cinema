using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Cinema.Application.Features.Sessions.Commands;
using Cinema.Application.Features.Sessions.Queries;
using Cinema.Application.Features.Sessions.ViewModels;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;
using Cinema.WebAPI.Controllers.Sessions;
using Cinema.WebAPI.Tests.Initializer;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Moq;
using NUnit.Framework;

namespace Cinema.WebAPI.Tests.Features.Sessions
{
    [TestFixture]
    class SessionControllerTests : TestWepAPIBase
    {
        private SessionsController _sessionsController;
        private Mock<ISessionService> _sessionServiceMock;

        private Mock<SessionAddCommand> _sessionAddCmd;
        private Mock<SessionGetAvailableLoungesQuery> _queryAvailableLounges;
        private Mock<Session> _session;

        private Mock<ValidationResult> _validator;

        [SetUp]
        public void Initialize()
        {
            SetupController();
            SetupCommands();
        }

        #region GET

        [Test]
        public void Sessions_Controller_Get_Should_Be_Ok()
        {
            // Arrange
            var id = 1;
            _session.Setup(x => x.Id).Returns(id);
            _session.Setup(x => x.Lounge).Returns(ObjectMother.GetDefaultLounge());
            _session.Setup(x => x.Movie).Returns(ObjectMother.GetDefaultMovie());
            var response = new List<Session>() { _session.Object }.AsQueryable();
            _sessionServiceMock.Setup(s => s.GetAll()).Returns(response);
            var odataOptions = GetOdataQueryOptions<Session>(_sessionsController);

            // Action
            var callback = _sessionsController.Get(odataOptions);

            // Assert
            _sessionServiceMock.Verify(s => s.GetAll(), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<SessionGridViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Sessions_Controller_Should_Get_By_Id_Sucessfully()
        {
            // Arrange
            var id = 1;
            _session.Setup(a => a.Id).Returns(id);
            _sessionServiceMock.Setup(c => c.GetById(id)).Returns(_session.Object);

            // Action
            IHttpActionResult callback = _sessionsController.GetById(id);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<SessionViewModel>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            httpResponse.Content.Id.Should().Be(id);
            _sessionServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        #endregion

        #region POST

        [Test]
        public void Sessions_Controller_Should_Add_Sucessfully()
        {
            // Arrange
            var id = 1;
            _session.Setup(l => l.Id).Returns(id);
            _sessionServiceMock.Setup(c => c.Add(_sessionAddCmd.Object)).Returns(_session.Object);

            // Action
            IHttpActionResult callback = _sessionsController.Post(_sessionAddCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<Session>>().Subject;
            httpResponse.Content.Should().Be(_session.Object);
            _sessionServiceMock.Verify(s => s.Add(_sessionAddCmd.Object), Times.Once);
        }

        [Test]
        public void Sessions_Controller_Add_Should_Handle_Validation_Errors()
        {
            //Arrange
            var notValid = false;
            _validator.Setup(v => v.IsValid).Returns(notValid);

            // Action
            var callback = _sessionsController.Post(_sessionAddCmd.Object);

            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _sessionAddCmd.Verify(cmd => cmd.Validate(_sessionServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _sessionAddCmd.VerifyNoOtherCalls();
        }

        [Test]
        public void Sessions_Controller_Should_Get_Available_Lounges()
        {
            // Arrange
            _sessionServiceMock.Setup(c => c.GetAvailableLounges(It.IsAny<Session>())).Returns(It.IsAny<IQueryable<Lounge>>());

            // Action
            IHttpActionResult callback = _sessionsController.GetAvailableLounges(_queryAvailableLounges.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<IQueryable<Lounge>>>().Subject;
            _sessionServiceMock.Verify(s => s.GetAvailableLounges(It.IsAny<Session>()), Times.Once);
            _sessionServiceMock.VerifyNoOtherCalls();
        }

        #endregion

        #region DELETE

        [Test]
        public void Sessions_Controller_Should_Remove_Sucessfully()
        {
            // Arrange
            var isUpdated = true;
            int idToRemove = 1;
            _sessionServiceMock.Setup(c => c.Remove(It.IsAny<long>())).Returns(isUpdated);

            // Action
            IHttpActionResult callback = _sessionsController.Delete(idToRemove);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            _sessionServiceMock.Verify(s => s.Remove(idToRemove), Times.Once);
            httpResponse.Content.Should().BeTrue();
        }

        #endregion

        private void SetupCommands()
        {
            _validator = new Mock<ValidationResult>();
            _sessionAddCmd = new Mock<SessionAddCommand>();
            _sessionAddCmd.Setup(cmd => cmd.Validate(_sessionServiceMock.Object)).Returns(_validator.Object);
            _session = new Mock<Session>();
            _queryAvailableLounges = new Mock<SessionGetAvailableLoungesQuery>();

            var valid = true;
            _validator.Setup(v => v.IsValid).Returns(valid);
        }

        private void SetupController()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            _sessionServiceMock = new Mock<ISessionService>();
            _sessionsController = new SessionsController(_mapper, _sessionServiceMock.Object)
            {
                Request = request,
            };
        }
    }
}
