using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Application.Features.Sessions;
using Cinema.Application.Tests.Initializer;
using Cinema.Common.Tests;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cinema.Application.Tests.Features.Sessions
{
    [TestFixture]
    class SessionServiceTest : TestServiceBase
    {
        ISessionService _sessionService;
        Mock<ILoungeService> _mockLoungeService;
        Mock<IMovieService> _mockMovieService;
        Mock<ISessionRepository> _mockSessionRepository;

        [SetUp]
        public void Initialize()
        {
            _mockLoungeService = new Mock<ILoungeService>();
            _mockSessionRepository = new Mock<ISessionRepository>();
            _mockMovieService = new Mock<IMovieService>();
            _sessionService = new SessionService(_mockMovieService.Object, _mockLoungeService.Object, _mockSessionRepository.Object, _mapper);
        }

        #region Add

        [Test]
        public void Session_Service_Should_Add_Session_Successfully()
        {
            //Arrange 
            var session = ObjectMother.GetDefaultSession();
            var sessionCmd = ObjectMother.GetSessionAddCommand();
            _mockSessionRepository.Setup(r => r.Add(It.IsAny<Session>())).Returns(session);
            _mockMovieService.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultMovie());
            _mockLoungeService.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultLounge());
            _mockLoungeService.Setup(r => r.GetAll()).Returns(new List<Lounge>() { ObjectMother.GetDefaultLounge() }.AsQueryable());

            //Action
            var addedSession = _sessionService.Add(sessionCmd);

            //Assert
            _mockSessionRepository.Verify(r => r.Add(It.IsAny<Session>()), Times.Once);
            addedSession.Id.Should().Be(session.Id);
        }

        [Test]
        public void Session_Service_Add_Should_Throw_Exception()
        {
            //Arrange
            var session = ObjectMother.GetDefaultSession();
            var sessionCmd = ObjectMother.GetSessionAddCommand();
            _mockSessionRepository.Setup(r => r.Add(It.IsAny<Session>())).Throws<Exception>();
            _mockMovieService.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultMovie());
            _mockLoungeService.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultLounge());
            _mockLoungeService.Setup(r => r.GetAll()).Returns(new List<Lounge>() { ObjectMother.GetDefaultLounge() }.AsQueryable());


            //Action
            Action act = () => _sessionService.Add(sessionCmd);

            //Assert
            act.Should().Throw<Exception>();
            _mockSessionRepository.Verify(r => r.Add(It.IsAny<Session>()), Times.Once);
        }

        [Test]
        public void Session_Service_Add_Should_Throw_Lounge_Not_Available_Exception()
        {
            //Arrange
            var session = ObjectMother.GetDefaultSession();
            var sessionCmd = ObjectMother.GetSessionAddCommand();
            _mockMovieService.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultMovie());
            _mockLoungeService.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultLounge());
            _mockLoungeService.Setup(r => r.GetAll()).Returns(new List<Lounge>() { }.AsQueryable());


            //Action
            Action act = () => _sessionService.Add(sessionCmd);

            //Assert
            act.Should().Throw<BusinessException>();
            _mockSessionRepository.Verify(r => r.Add(It.IsAny<Session>()), Times.Never);
        }
        #endregion

        #region Remove

        [Test]
        public void Session_Service_Should_Remove_Session_Sucessfully()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = true;
            _mockSessionRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _sessionService.Remove(idToRemove);

            //Assert
            _mockSessionRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeTrue();
        }

        [Test]
        public void Session_Service_Should_Return_False_When_Failed_Remove()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = false;
            _mockSessionRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _sessionService.Remove(idToRemove);

            //Assert
            _mockSessionRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeFalse();
        }
        #endregion

        #region Get

        [Test]
        public void Session_Service_Should_Get_Session_By_Id_Sucessfully()
        {
            //Arrange
            var session = ObjectMother.GetDefaultSession();
            _mockSessionRepository.Setup(r => r.GetById(session.Id)).Returns(session);

            //Action
            var returnedSession = _sessionService.GetById(session.Id);

            //Assert
            _mockSessionRepository.Verify(r => r.GetById(session.Id), Times.Once);
            returnedSession.Should().NotBeNull();
            returnedSession.Id.Should().Be(session.Id);
        }

        [Test]
        public void Session_Service_Should_Get_All_Sessions_Sucessfully()
        {
            //Arrange
            var session = ObjectMother.GetDefaultSession();
            var mockValueRepository = new List<Session>() { session }.AsQueryable();
            _mockSessionRepository.Setup(r => r.GetAll()).Returns(mockValueRepository);

            //Action
            var sessions = _sessionService.GetAll();

            //Assert
            _mockSessionRepository.Verify(r => r.GetAll(), Times.Once);
            sessions.Should().NotBeNull();
            sessions.First().Should().Be(session);
            sessions.Count().Should().Be(mockValueRepository.Count());
        }

        [Test]
        public void Session_Service_Should_Get_Available_Lounges_Should_Return_0()
        {
            //Arrange
            var futureSession = ObjectMother.GetDefaultSession();
            futureSession.Start.AddMinutes(5);
            var session = ObjectMother.GetDefaultSession();
            _mockSessionRepository.Setup(r => r.GetAll()).Returns(new List<Session>() { futureSession }.AsQueryable());
            var returnMockLounge = ObjectMother.GetDefaultLounge();
            returnMockLounge.Id = 5; // qualquer id diferente de 1
            _mockLoungeService.Setup(r => r.GetAll()).Returns(new List<Lounge> {  }.AsQueryable());

            //Action
            var result = _sessionService.GetAvailableLounges(session);

            //Assert
            _mockSessionRepository.Verify(r => r.GetAll(), Times.Once);
            _mockLoungeService.Verify(r => r.GetAll(), Times.Once);
            result.Should().BeEmpty();
        }
        #endregion
    }
}