using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Cinema.Application.Features.Lounges.Commands;
using Cinema.Application.Features.Lounges.Queries;
using Cinema.Application.Features.Lounges.ViewModels;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.WebAPI.Controllers.Lounges;
using Cinema.WebAPI.Tests.Initializer;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Moq;
using NUnit.Framework;

namespace Cinema.WebAPI.Tests.Features.Lounges
{
    [TestFixture]
    class LoungeControllerTests : TestWepAPIBase
    {
        private LoungesController _loungesController;
        private Mock<ILoungeService> _loungeServiceMock;

        private Mock<LoungeUpdateCommand> _loungeUpdateCmd;
        private Mock<LoungeAddCommand> _loungeAddCmd;
        private Mock<LoungeCheckNameQuery> _loungeNameQuery;
        private Mock<Lounge> _lounge;

        private Mock<ValidationResult> _validator;

        [SetUp]
        public void Initialize()
        {
            SetupController();
            SetupCommands();
        }

        #region GET

        [Test]
        public void Lounges_Controller_Get_Should_Be_Ok()
        {
            // Arrange
            long id = 1;
            _lounge.Setup(x => x.Id).Returns(id);
            var response = new List<Lounge>() { _lounge.Object }.AsQueryable();
            _loungeServiceMock.Setup(s => s.GetAll()).Returns(response);
            var odataOptions = GetOdataQueryOptions<Lounge>(_loungesController);

            // Action
            var callback = _loungesController.Get(odataOptions);

            // Assert
            _loungeServiceMock.Verify(s => s.GetAll(), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<LoungeViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Lounges_Controller_Should_Get_By_Id_Sucessfully()
        {
            // Arrange
            var id = 1;
            _lounge.Setup(a => a.Id).Returns(id);
            _loungeServiceMock.Setup(c => c.GetById(id)).Returns(_lounge.Object);

            // Action
            IHttpActionResult callback = _loungesController.GetById(id);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<LoungeViewModel>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            httpResponse.Content.Id.Should().Be(id);
            _loungeServiceMock.Verify(s => s.GetById(id), Times.Once);
        }

        #endregion

        #region POST

        [Test]
        public void Lounges_Controller_Should_Add_Sucessfully()
        {
            // Arrange
            var id = 1;
            _lounge.Setup(l => l.Id).Returns(id);
            _loungeServiceMock.Setup(c => c.Add(_loungeAddCmd.Object)).Returns(_lounge.Object);

            // Action
            IHttpActionResult callback = _loungesController.Post(_loungeAddCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<Lounge>>().Subject;
            httpResponse.Content.Should().Be(_lounge.Object);
            _loungeServiceMock.Verify(s => s.Add(_loungeAddCmd.Object), Times.Once);
        }

        [Test]
        public void Lounges_Controller_Add_Should_Handle_Validation_Errors()
        {
            //Arrange
            var notValid = false;
            _validator.Setup(v => v.IsValid).Returns(notValid);

            // Action
            var callback = _loungesController.Post(_loungeAddCmd.Object);

            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _loungeAddCmd.Verify(cmd => cmd.Validate(_loungeServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _loungeAddCmd.VerifyNoOtherCalls();
        }

        [Test]
        public void Lounges_Controller_Should_Verify_Available_Name()
        {
            // Arrange
            bool available = true;
            _loungeServiceMock.Setup(c => c.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>())).Returns(available);

            // Action
            IHttpActionResult callback = _loungesController.CheckName(_loungeNameQuery.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().Be(available);
            _loungeServiceMock.Verify(s => s.IsNameAlreadyInUse(It.IsAny<string>(), It.IsAny<long>()), Times.Once);
            _loungeServiceMock.VerifyNoOtherCalls();
        }

        #endregion

        #region PATCH

        [Test]
        public void Lounges_Controller_Should_Update_Sucessfully()
        {
            // Arrange
            var successUpdate = true;
            _loungeServiceMock.Setup(c => c.Update(_loungeUpdateCmd.Object)).Returns(successUpdate);

            // Action
            IHttpActionResult callback = _loungesController.Update(_loungeUpdateCmd.Object);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().BeTrue();
            _loungeServiceMock.Verify(s => s.Update(_loungeUpdateCmd.Object), Times.Once);
        }

        [Test]
        public void Lounges_Controller_Update_Should_Handle_Validation_Errors()
        {
            //Arrange
            var isValid = false;
            _validator.Setup(v => v.IsValid).Returns(isValid);
            // Action
            var callback = _loungesController.Update(_loungeUpdateCmd.Object);
            //Assert
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            _loungeUpdateCmd.Verify(cmd => cmd.Validate(_loungeServiceMock.Object), Times.Once);
            _validator.Verify(x => x.IsValid, Times.Once);
            _loungeUpdateCmd.VerifyNoOtherCalls();
        }

        #endregion

        #region DELETE

        [Test]
        public void Lounges_Controller_Should_Remove_Sucessfully()
        {
            // Arrange
            var isUpdated = true;
            int idToRemove = 1;
            _loungeServiceMock.Setup(c => c.Remove(It.IsAny<long>())).Returns(isUpdated);

            // Action
            IHttpActionResult callback = _loungesController.Delete(idToRemove);

            // Assert
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            _loungeServiceMock.Verify(s => s.Remove(idToRemove), Times.Once);
            httpResponse.Content.Should().BeTrue();
        }

        #endregion

        private void SetupCommands()
        {
            _validator = new Mock<ValidationResult>();
            _loungeUpdateCmd = new Mock<LoungeUpdateCommand>();
            _loungeUpdateCmd.Setup(cmd => cmd.Validate(_loungeServiceMock.Object)).Returns(_validator.Object);
            _loungeAddCmd = new Mock<LoungeAddCommand>();
            _loungeAddCmd.Setup(cmd => cmd.Validate(_loungeServiceMock.Object)).Returns(_validator.Object);
            _lounge = new Mock<Lounge>();
            _loungeNameQuery = new Mock<LoungeCheckNameQuery>();

            var valid = true;
            _validator.Setup(v => v.IsValid).Returns(valid);
        }

        private void SetupController()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            _loungeServiceMock = new Mock<ILoungeService>();
            _loungesController = new LoungesController(_mapper, _loungeServiceMock.Object)
            {
                Request = request,
            };
        }
    }
}
