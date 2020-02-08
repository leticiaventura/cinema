using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Cinema.Application.Features.Snacks.Commands;
using Cinema.Application.Features.Snacks.Queries;
using Cinema.Application.Features.Snacks.ViewModels;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;
using Cinema.WebAPI.Controllers.Snacks;
using Cinema.WebAPI.Tests.Initializer;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Moq;
using NUnit.Framework;

namespace Cinema.WebAPI.Tests.Features.Snacks
{
    [TestFixture]
    class SnackControllerTests : TestWepAPIBase
    {
        private SnacksController _snacksController;
        private Mock<ISnackService> _snackServiceMock;

        private Mock<SnackUpdateCommand> _snackUpdateCmd;
        private Mock<SnackAddCommand> _snackAddCmd;
        private Mock<SnackCheckNameQuery> _snackNameQuery;
        private Mock<Snack> _snack;

        private Mock<ValidationResult> _validator;

        [SetUp]
        public void Initialize()
        {
            SetupController();
            SetupCommands();
        }

        #region GET

        [Test]
        public void Snacks_Controller_Get_Should_Be_Ok()
        {
            // Arrange
            var id = 1;
            _snack.Setup(x => x.Id).Returns(id);
            var response = new List<Snack>() { _snack.Object }.AsQueryable();
            _snackServiceMock.Setup(s => s.GetAll()).Returns(response);
            var odataOptions = GetOdataQueryOptions<Snack>(_snacksController);

            // Action
            var callback = _snacksController.Get(odataOptions);

            // Assert
            _snackServiceMock.Verify(s => s.GetAll(), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<SnackGridViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Snacks_Controller_Should_Get_By_Id_Sucessfully()
        {
            // Arrange
            var id = 1;
            _snack.Setup(a => a.Id).Returns(id);
            _snackServiceMock.Setup(c => c.GetById(id)).Returns(_snack.Object);

            // Action
            IHttpActionResult callback = _snacksController.GetById(id);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<SnackViewModel>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            httpResponse.Content.Id.Should().Be(id);
            _snackServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        #endregion

        #region POST

        [Test]
        public void Snacks_Controller_Should_Add_Sucessfully()
        {
            // Arrange
            var id = 1;
            _snack.Setup(l => l.Id).Returns(id);
            _snackServiceMock.Setup(c => c.Add(_snackAddCmd.Object)).Returns(_snack.Object);

            // Action
            IHttpActionResult callback = _snacksController.Post(_snackAddCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<Snack>>().Subject;
            httpResponse.Content.Should().Be(_snack.Object);
            _snackServiceMock.Verify(s => s.Add(_snackAddCmd.Object), Times.Once);
        }

        [Test]
        public void Snacks_Controller_Add_Should_Handle_Validation_Errors()
        {
            //Arrange
            var notValid = false;
            _validator.Setup(v => v.IsValid).Returns(notValid);

            // Action
            var callback = _snacksController.Post(_snackAddCmd.Object);

            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _snackAddCmd.Verify(cmd => cmd.Validate(_snackServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _snackAddCmd.VerifyNoOtherCalls();
        }

        [Test]
        public void Snacks_Controller_Should_Verify_Available_Name()
        {
            // Arrange
            bool available = true;
            _snackServiceMock.Setup(c => c.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(available);

            // Action
            IHttpActionResult callback = _snacksController.CheckName(_snackNameQuery.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().Be(available);
            _snackServiceMock.Verify(s => s.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>()), Times.Once);
            _snackServiceMock.VerifyNoOtherCalls();
        }

        #endregion

        #region PATCH

        [Test]
        public void Snacks_Controller_Should_Update_Sucessfully()
        {
            // Arrange
            var successUpdate = true;
            _snackServiceMock.Setup(c => c.Update(_snackUpdateCmd.Object)).Returns(successUpdate);

            // Action
            IHttpActionResult callback = _snacksController.Update(_snackUpdateCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().BeTrue();
            _snackServiceMock.Verify(s => s.Update(_snackUpdateCmd.Object), Times.Once);
        }

        [Test]
        public void Snacks_Controller_Update_Should_Handle_Validation_Errors()
        {
            //Arrange
            var isValid = false;
            _validator.Setup(v => v.IsValid).Returns(isValid);
            // Action
            var callback = _snacksController.Update(_snackUpdateCmd.Object);
            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _snackUpdateCmd.Verify(cmd => cmd.Validate(_snackServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _snackUpdateCmd.VerifyNoOtherCalls();
        }

        #endregion

        #region DELETE

        [Test]
        public void Snacks_Controller_Should_Remove_Sucessfully()
        {
            // Arrange
            var isUpdated = true;
            int idToRemove = 1;
            _snackServiceMock.Setup(c => c.Remove(It.IsAny<long>())).Returns(isUpdated);

            // Action
            IHttpActionResult callback = _snacksController.Delete(idToRemove);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            _snackServiceMock.Verify(s => s.Remove(idToRemove), Times.Once);
            httpResponse.Content.Should().BeTrue();
        }

        #endregion

        private void SetupCommands()
        {
            _validator = new Mock<ValidationResult>();
            _snackUpdateCmd = new Mock<SnackUpdateCommand>();
            _snackUpdateCmd.Setup(cmd => cmd.Validate(_snackServiceMock.Object)).Returns(_validator.Object);
            _snackAddCmd = new Mock<SnackAddCommand>();
            _snackAddCmd.Setup(cmd => cmd.Validate(_snackServiceMock.Object)).Returns(_validator.Object);
            _snack = new Mock<Snack>();
            _snackNameQuery = new Mock<SnackCheckNameQuery>();

            var valid = true;
            _validator.Setup(v => v.IsValid).Returns(valid);
        }

        private void SetupController()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            _snackServiceMock = new Mock<ISnackService>();
            _snacksController = new SnacksController(_mapper, _snackServiceMock.Object)
            {
                Request = request,
            };
        }
    }
}
