using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Application.Features.Snacks;
using Cinema.Application.Tests.Initializer;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cinema.Application.Tests.Features.Snacks
{
    [TestFixture]
    class SnackServiceTest : TestServiceBase
    {
        ISnackService _snackService;
        Mock<ISnackRepository> _mockSnackRepository;

        [SetUp]
        public void Initialize()
        {
            _mockSnackRepository = new Mock<ISnackRepository>();
            _snackService = new SnackService(_mockSnackRepository.Object, _mapper);
        }

        #region Add

        [Test]
        public void Should_Add_Snack_Successfully()
        {
            //Arrange 
            var snack = ObjectMother.GetDefaultSnack();
            var snackCmd = ObjectMother.GetSnackAddCommand();
            _mockSnackRepository.Setup(r => r.Add(It.IsAny<Snack>())).Returns(snack);

            //Action
            var addedSnack = _snackService.Add(snackCmd);

            //Assert
            _mockSnackRepository.Verify(r => r.Add(It.IsAny<Snack>()), Times.Once);
            addedSnack.Id.Should().Be(snack.Id);
        }

        [Test]
        public void Snack_Service_Add_Should_Throw_Exception()
        {
            //Arrange
            var snackCmd = ObjectMother.GetSnackAddCommand();
            _mockSnackRepository.Setup(r => r.Add(It.IsAny<Snack>())).Throws<Exception>();

            //Action
            Action act = () => _snackService.Add(snackCmd);

            //Assert
            act.Should().Throw<Exception>();
            _mockSnackRepository.Verify(r => r.Add(It.IsAny<Snack>()), Times.Once);
        }
        #endregion

        #region Update
        [Test]
        public void Should_Update_Snack_Sucessfully()
        {
            //Arrange
            var snack = ObjectMother.GetDefaultSnack();
            var snackCmd = ObjectMother.GetSnackUpdateCommand();
            var updated = true;
            _mockSnackRepository.Setup(e => e.GetById(snackCmd.Id)).Returns(snack);
            _mockSnackRepository.Setup(r => r.Update(snack)).Returns(updated);

            //Action
            var updateSnack = _snackService.Update(snackCmd);

            //Assert
            _mockSnackRepository.Verify(e => e.GetById(snackCmd.Id), Times.Once);
            _mockSnackRepository.Verify(r => r.Update(snack), Times.Once);
            updateSnack.Should().BeTrue();
        }

        [Test]
        public void Snack_Service_Update_Should_Not_Run_When_Not_Found()
        {
            //Arrange
            var snackCmd = ObjectMother.GetSnackUpdateCommand();
            _mockSnackRepository.Setup(e => e.GetById(snackCmd.Id)).Returns((Snack)null);

            //Action
            var updateSnack = _snackService.Update(snackCmd);

            //Assert
            _mockSnackRepository.Verify(e => e.GetById(snackCmd.Id), Times.Once);
            _mockSnackRepository.Verify(e => e.Update(It.IsAny<Snack>()), Times.Never);
            updateSnack.Should().BeFalse();
        }

        #endregion

        #region Remove

        [Test]
        public void Should_Remove_Snack_Sucessfully()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = true;
            _mockSnackRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _snackService.Remove(idToRemove);

            //Assert
            _mockSnackRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeTrue();
        }

        [Test]
        public void Snack_Service_Should_Return_False_When_Failed_Remove()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = false;
            _mockSnackRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _snackService.Remove(idToRemove);

            //Assert
            _mockSnackRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeFalse();
        }
        #endregion

        #region Get

        [Test]
        public void Should_Get_Snack_By_Id_Sucessfully()
        {
            //Arrange
            var snack = ObjectMother.GetDefaultSnack();
            _mockSnackRepository.Setup(r => r.GetById(snack.Id)).Returns(snack);

            //Action
            var returnedSnack = _snackService.GetById(snack.Id);

            //Assert
            _mockSnackRepository.Verify(r => r.GetById(snack.Id), Times.Once);
            returnedSnack.Should().NotBeNull();
            returnedSnack.Id.Should().Be(snack.Id);
        }

        [Test]
        public void Should_Get_All_Snacks_Sucessfully()
        {
            //Arrange
            var snack = ObjectMother.GetDefaultSnack();
            var mockValueRepository = new List<Snack>() { snack }.AsQueryable();
            _mockSnackRepository.Setup(r => r.GetAll()).Returns(mockValueRepository);

            //Action
            var snacks = _snackService.GetAll();

            //Assert
            _mockSnackRepository.Verify(r => r.GetAll(), Times.Once);
            snacks.Should().NotBeNull();
            snacks.First().Should().Be(snack);
            snacks.Count().Should().Be(mockValueRepository.Count());
        }

        [Test]
        public void Should_Verify_Snack_Name_Sucessfully()
        {
            //Arrange
            var snack = ObjectMother.GetDefaultSnack();
            _mockSnackRepository.Setup(r => r.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(false);

            //Action
            var result = _snackService.IsNameAlreadyInUse(snack.Name, snack.Id);

            //Assert
            _mockSnackRepository.Verify(r => r.IsNameAlreadyInUse(snack.Name, snack.Id), Times.Once);
            result.Should().BeFalse();
        }

        [Test]
        public void Should_Get_All_Snacks_With_Quantity_Sucessfully()
        {
            //Arrange
            int quantity = 2;
            var snack = ObjectMother.GetDefaultSnack();
            var mockValueRepository = new List<Snack>() { snack, snack }.AsQueryable();
            _mockSnackRepository.Setup(r => r.GetAll(It.IsAny<int>())).Returns(mockValueRepository);

            //Action
            var snacks = _snackService.GetAll(quantity);

            //Assert
            _mockSnackRepository.Verify(r => r.GetAll(It.IsAny<int>()), Times.Once);
            snacks.Should().NotBeNull();
            snacks.First().Should().Be(snack);
            snacks.Count().Should().Be(mockValueRepository.Count());
        }
        #endregion
    }
}