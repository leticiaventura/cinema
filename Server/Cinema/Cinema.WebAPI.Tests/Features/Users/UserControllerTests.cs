using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Cinema.Application.Features.Users.Commands;
using Cinema.Application.Features.Users.Queries;
using Cinema.Application.Features.Users.ViewModels;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using Cinema.WebAPI.Controllers.Users;
using Cinema.WebAPI.Tests.Initializer;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Moq;
using NUnit.Framework;

namespace Cinema.WebAPI.Tests.Features.Users
{
    [TestFixture]
    class UserControllerTests : TestWepAPIBase
    {
        private UsersController _usersController;
        private Mock<IUserService> _userServiceMock;

        private Mock<UserUpdateCommand> _userUpdateCmd;
        private Mock<UserAddCommand> _userAddCmd;
        private Mock<UserCheckNameQuery> _userNameQuery;
        private Mock<User> _user;

        private Mock<ValidationResult> _validator;

        [SetUp]
        public void Initialize()
        {
            SetupController();
            SetupCommands();
        }

        #region GET

        [Test]
        public void Users_Controller_Get_Should_Be_Ok()
        {
            // Arrange
            var user = ObjectMother.GetDefaultUser();
            var response = new List<User>() { user }.AsQueryable();
            _userServiceMock.Setup(s => s.GetAll()).Returns(response);
            var id = 1;
            _userUpdateCmd.Setup(a => a.Id).Returns(id);
            var odataOptions = GetOdataQueryOptions<User>(_usersController);

            // Action
            var callback = _usersController.Get(odataOptions);

            // Assert
            _userServiceMock.Verify(s => s.GetAll(), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<UserViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Users_Controller_Should_Get_By_Id_Sucessfully()
        {
            // Arrange
            var id = 1;
            _userUpdateCmd.Setup(a => a.Id).Returns(id);
            _user.Setup(a => a.Id).Returns(id);
            _userServiceMock.Setup(c => c.GetById(id)).Returns(_user.Object);

            // Action
            IHttpActionResult callback = _usersController.GetById(id);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<UserViewModel>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            httpResponse.Content.Id.Should().Be(id);
            _userServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        #endregion

        #region POST

        [Test]
        public void Users_Controller_Should_Add_Sucessfully()
        {
            // Arrange
            var id = 1;
            _user.Setup(l => l.Id).Returns(id);
            _userServiceMock.Setup(c => c.Add(_userAddCmd.Object)).Returns(_user.Object);

            // Action
            IHttpActionResult callback = _usersController.Post(_userAddCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<User>>().Subject;
            httpResponse.Content.Should().Be(_user.Object);
            _userServiceMock.Verify(s => s.Add(_userAddCmd.Object), Times.Once);
        }

        [Test]
        public void Users_Controller_Add_Should_Handle_Validation_Errors()
        {
            //Arrange
            var notValid = false;
            _validator.Setup(v => v.IsValid).Returns(notValid);

            // Action
            var callback = _usersController.Post(_userAddCmd.Object);

            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _userAddCmd.Verify(cmd => cmd.Validate(_userServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _userAddCmd.VerifyNoOtherCalls();
        }

        [Test]
        public void Users_Controller_Should_Verify_Available_Name()
        {
            // Arrange
            bool available = true;
            _userServiceMock.Setup(c => c.IsEmailAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(available);

            // Action
            IHttpActionResult callback = _usersController.CheckEmail(_userNameQuery.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().Be(available);
            _userServiceMock.Verify(s => s.IsEmailAlreadyInUse(It.IsAny<string>(), It.IsAny<long>()), Times.Once);
            _userServiceMock.VerifyNoOtherCalls();
        }

        #endregion

        #region PATCH

        [Test]
        public void Users_Controller_Should_Update_Sucessfully()
        {
            // Arrange
            var successUpdate = true;
            _userServiceMock.Setup(c => c.Update(_userUpdateCmd.Object)).Returns(successUpdate);

            // Action
            IHttpActionResult callback = _usersController.Update(_userUpdateCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().BeTrue();
            _userServiceMock.Verify(s => s.Update(_userUpdateCmd.Object), Times.Once);
        }

        [Test]
        public void Users_Controller_Update_Should_Handle_Validation_Errors()
        {
            //Arrange
            var isValid = false;
            _validator.Setup(v => v.IsValid).Returns(isValid);
            // Action
            var callback = _usersController.Update(_userUpdateCmd.Object);
            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _userUpdateCmd.Verify(cmd => cmd.Validate(_userServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _userUpdateCmd.VerifyNoOtherCalls();
        }

        #endregion

        #region DELETE

        [Test]
        public void Users_Controller_Should_Remove_Sucessfully()
        {
            // Arrange
            var isUpdated = true;
            int idToRemove = 1;
            _userServiceMock.Setup(c => c.Remove(It.IsAny<long>())).Returns(isUpdated);

            // Action
            IHttpActionResult callback = _usersController.Delete(idToRemove);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            _userServiceMock.Verify(s => s.Remove(idToRemove), Times.Once);
            httpResponse.Content.Should().BeTrue();
        }

        #endregion

        private void SetupCommands()
        {
            _validator = new Mock<ValidationResult>();
            _userUpdateCmd = new Mock<UserUpdateCommand>();
            _userUpdateCmd.Setup(cmd => cmd.Validate(_userServiceMock.Object)).Returns(_validator.Object);
            _userAddCmd = new Mock<UserAddCommand>();
            _userAddCmd.Setup(cmd => cmd.Validate(_userServiceMock.Object)).Returns(_validator.Object);
            _user = new Mock<User>();
            _userNameQuery = new Mock<UserCheckNameQuery>();

            var valid = true;
            _validator.Setup(v => v.IsValid).Returns(valid);
        }

        private void SetupController()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            _userServiceMock = new Mock<IUserService>();
            _usersController = new UsersController(_mapper, _userServiceMock.Object)
            {
                Request = request,
            };
        }
    }
}
