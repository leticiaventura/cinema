using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Application.Features.Lounges;
using Cinema.Application.Tests.Initializer;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.Domain.Features.Sessions.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cinema.Application.Tests.Features.Lounges
{
    [TestFixture]
    class LoungeServiceTest : TestServiceBase
    {
        ILoungeService _loungeService;
        Mock<ILoungeRepository> _mockLoungeRepository;
        Mock<ISessionRepository> _mockSessionRepository;

        [SetUp]
        public void Initialize()
        {
            _mockLoungeRepository = new Mock<ILoungeRepository>();
            _mockSessionRepository = new Mock<ISessionRepository>();
            _loungeService = new LoungeService(_mockSessionRepository.Object, _mockLoungeRepository.Object, _mapper);
        }

        #region Add

        [Test]
        public void Lounge_Service_Should_Add_Lounge_Successfully()
        {
            //Arrange 
            var lounge = ObjectMother.GetDefaultLounge();
            var loungeCmd = ObjectMother.GetLoungeAddCommand();
            _mockLoungeRepository.Setup(r => r.Add(It.IsAny<Lounge>())).Returns(lounge);

            //Action
            var addedLounge = _loungeService.Add(loungeCmd);

            //Assert
            _mockLoungeRepository.Verify(r => r.Add(It.IsAny<Lounge>()), Times.Once);
            addedLounge.Id.Should().Be(lounge.Id);
        }

        [Test]
        public void Lounge_Service_Add_Should_Throw_Exception()
        {
            //Arrange
            var loungeCmd = ObjectMother.GetLoungeAddCommand();
            _mockLoungeRepository.Setup(r => r.Add(It.IsAny<Lounge>())).Throws<Exception>();

            //Action
            Action act = () => _loungeService.Add(loungeCmd);

            //Assert
            act.Should().Throw<Exception>();
            _mockLoungeRepository.Verify(r => r.Add(It.IsAny<Lounge>()), Times.Once);
        }
        #endregion

        #region Update
        [Test]
        public void Lounge_Service_Should_Update_Lounge_Sucessfully()
        {
            //Arrange
            var lounge = ObjectMother.GetDefaultLounge();
            var loungeCmd = ObjectMother.GetLoungeUpdateCommand();
            var updated = true;
            _mockLoungeRepository.Setup(e => e.GetById(loungeCmd.Id)).Returns(lounge);
            _mockLoungeRepository.Setup(r => r.Update(lounge)).Returns(updated);

            //Action
            var updateLounge = _loungeService.Update(loungeCmd);

            //Assert
            _mockLoungeRepository.Verify(e => e.GetById(loungeCmd.Id), Times.Once);
            _mockLoungeRepository.Verify(r => r.Update(lounge), Times.Once);
            updateLounge.Should().BeTrue();
        }

        [Test]
        public void Lounge_Service_Update_Should_Not_Run_When_Not_Found()
        {
            //Arrange
            var loungeCmd = ObjectMother.GetLoungeUpdateCommand();
            _mockLoungeRepository.Setup(e => e.GetById(loungeCmd.Id)).Returns((Lounge)null);

            //Action
            var updateLounge = _loungeService.Update(loungeCmd);

            //Assert
            _mockLoungeRepository.Verify(e => e.GetById(loungeCmd.Id), Times.Once);
            _mockLoungeRepository.Verify(e => e.Update(It.IsAny<Lounge>()), Times.Never);
            updateLounge.Should().BeFalse();
        }

        #endregion

        #region Remove

        [Test]
        public void Lounge_Service_Should_Remove_Lounge_Sucessfully()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = true;
            _mockLoungeRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _loungeService.Remove(idToRemove);

            //Assert
            _mockLoungeRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeTrue();
        }

        [Test]
        public void Lounge_Service_Should_Return_False_When_Failed_Remove()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = false;
            _mockLoungeRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _loungeService.Remove(idToRemove);

            //Assert
            _mockLoungeRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeFalse();
        }
        #endregion

        #region Get

        [Test]
        public void Lounge_Service_Should_Get_Lounge_By_Id_Sucessfully()
        {
            //Arrange
            var lounge = ObjectMother.GetDefaultLounge();
            _mockLoungeRepository.Setup(r => r.GetById(lounge.Id)).Returns(lounge);

            //Action
            var returnedLounge = _loungeService.GetById(lounge.Id);

            //Assert
            _mockLoungeRepository.Verify(r => r.GetById(lounge.Id), Times.Once);
            returnedLounge.Should().NotBeNull();
            returnedLounge.Id.Should().Be(lounge.Id);
        }

        [Test]
        public void Lounge_Service_Should_Get_All_Lounges_Sucessfully()
        {
            //Arrange
            var lounge = ObjectMother.GetDefaultLounge();
            var mockValueRepository = new List<Lounge>() { lounge }.AsQueryable();
            _mockLoungeRepository.Setup(r => r.GetAll()).Returns(mockValueRepository);

            //Action
            var lounges = _loungeService.GetAll();

            //Assert
            _mockLoungeRepository.Verify(r => r.GetAll(), Times.Once);
            lounges.Should().NotBeNull();
            lounges.First().Should().Be(lounge);
            lounges.Count().Should().Be(mockValueRepository.Count());
        }

        [Test]
        public void Lounge_Service_Should_Verify_Lounge_Name_Sucessfully()
        {
            //Arrange
            var lounge = ObjectMother.GetDefaultLounge();
            _mockLoungeRepository.Setup(r => r.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(false);

            //Action
            var result = _loungeService.IsNameAlreadyInUse(lounge.Name, lounge.Id);

            //Assert
            _mockLoungeRepository.Verify(r => r.IsNameAlreadyInUse(lounge.Name, lounge.Id), Times.Once);
            result.Should().BeFalse();
        }
        #endregion
    }
}