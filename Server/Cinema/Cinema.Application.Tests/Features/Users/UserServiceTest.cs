using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinema.Application.Features.Users;
using Cinema.Application.Features.Users.Commands;
using Cinema.Application.Tests.Initializer;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cinema.Application.Tests.Features.Users
{
    [TestFixture]
    class UserServiceTest : TestServiceBase
    {
        IUserService _userService;
        Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Initialize()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object, _mapper);
        }

        #region Add

        [Test]
        public void User_Service_Should_Add_User_Successfully()
        {
            //Arrange 
            var user = ObjectMother.GetDefaultUser();
            var userCmd = ObjectMother.GetUserAddCommand();
            _mockUserRepository.Setup(r => r.Add(It.IsAny<User>())).Returns(user);

            //Action
            var addedUser = _userService.Add(userCmd);

            //Assert
            _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
            addedUser.Id.Should().Be(user.Id);
        }

        [Test]
        public void User_Service_Add_Should_Throw_Exception()
        {
            //Arrange
            var userCmd = ObjectMother.GetUserAddCommand();
            _mockUserRepository.Setup(r => r.Add(It.IsAny<User>())).Throws<Exception>();

            //Action
            Action act = () => _userService.Add(userCmd);

            //Assert
            act.Should().Throw<Exception>();
            _mockUserRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }
        #endregion

        #region Update
        [Test]
        public void User_Service_Should_Update_User_Sucessfully()
        {
            //Arrange
            var user = ObjectMother.GetDefaultUser();
            var userCmd = ObjectMother.GetUserUpdateCommand();
            var updated = true;
            _mockUserRepository.Setup(e => e.GetById(userCmd.Id)).Returns(user);
            _mockUserRepository.Setup(r => r.Update(user)).Returns(updated);

            //Action
            var updateUser = _userService.Update(userCmd);

            //Assert
            _mockUserRepository.Verify(e => e.GetById(userCmd.Id), Times.Once);
            _mockUserRepository.Verify(r => r.Update(user), Times.Once);
            updateUser.Should().BeTrue();
        }

        [Test]
        public void User_Service_Update_Should_Not_Run_When_Not_Found()
        {
            //Arrange
            var userCmd = ObjectMother.GetUserUpdateCommand();
            _mockUserRepository.Setup(e => e.GetById(userCmd.Id)).Returns((User)null);

            //Action
            var updateUser = _userService.Update(userCmd);

            //Assert
            _mockUserRepository.Verify(e => e.GetById(userCmd.Id), Times.Once);
            _mockUserRepository.Verify(e => e.Update(It.IsAny<User>()), Times.Never);
            updateUser.Should().BeFalse();
        }

        #endregion

        #region Remove

        [Test]
        public void User_Service_Should_Remove_User_Sucessfully()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = true;
            _mockUserRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _userService.Remove(idToRemove);

            //Assert
            _mockUserRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeTrue();
        }

        [Test]
        public void User_Service_Should_Return_False_When_Failed_Remove()
        {
            //Arrange
            var idToRemove = 1;
            var mockWasRemoved = false;
            _mockUserRepository.Setup(e => e.Remove(idToRemove)).Returns(mockWasRemoved);

            //Action
            var removed = _userService.Remove(idToRemove);

            //Assert
            _mockUserRepository.Verify(e => e.Remove(idToRemove), Times.Once);
            removed.Should().BeFalse();
        }
        #endregion

        #region Get

        [Test]
        public void User_Service_Should_Get_User_By_Id_Sucessfully()
        {
            //Arrange
            var user = ObjectMother.GetDefaultUser();
            _mockUserRepository.Setup(r => r.GetById(user.Id)).Returns(user);

            //Action
            var returnedUser = _userService.GetById(user.Id);

            //Assert
            _mockUserRepository.Verify(r => r.GetById(user.Id), Times.Once);
            returnedUser.Should().NotBeNull();
            returnedUser.Id.Should().Be(user.Id);
        }     
        
        [Test]
        public void User_Service_Should_Login_Successfully()
        {
            //Arrange
            var user = ObjectMother.GetDefaultUser();
            _mockUserRepository.Setup(r => r.GetByCredentials(It.IsAny<string>(), It.IsAny<string>())).Returns(user);

            //Action
            var returnedUser = _userService.Login(user.Email, user.Password);

            //Assert
            _mockUserRepository.Verify(r => r.GetByCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            returnedUser.Should().NotBeNull();
            returnedUser.Id.Should().Be(user.Id);
        }

        [Test]
        public void User_Service_Should_Get_All_Users_Sucessfully()
        {
            //Arrange
            var user = ObjectMother.GetDefaultUser();
            var mockValueRepository = new List<User>() { user }.AsQueryable();
            _mockUserRepository.Setup(r => r.GetAll()).Returns(mockValueRepository);

            //Action
            var users = _userService.GetAll();

            //Assert
            _mockUserRepository.Verify(r => r.GetAll(), Times.Once);
            users.Should().NotBeNull();
            users.First().Should().Be(user);
            users.Count().Should().Be(mockValueRepository.Count());
        }    

        [Test]
        public void User_Service_Should_Verify_User_Email_Sucessfully()
        {
            //Arrange
            var user = ObjectMother.GetDefaultUser();
            _mockUserRepository.Setup(r => r.IsEmailAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(false);

            //Action
            var result = _userService.IsEmailAlreadyInUse(user.Email, user.Id);

            //Assert
            _mockUserRepository.Verify(r => r.IsEmailAlreadyInUse(user.Email, user.Id), Times.Once);
            result.Should().BeFalse();
        }
        #endregion
    }
}
