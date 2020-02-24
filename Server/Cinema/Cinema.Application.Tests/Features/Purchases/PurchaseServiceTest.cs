using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Application.Features.Purchases;
using Cinema.Application.Tests.Initializer;
using Cinema.Common.Tests;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Purchases.Interfaces;
using Cinema.Domain.Features.Sessions.Interfaces;
using Cinema.Domain.Features.Users.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cinema.Application.Tests.Features.Purchases
{
    [TestFixture]
    class PurchaseServiceTest : TestServiceBase
    {
        IPurchaseService _purchaseService;
        Mock<IPurchaseRepository> _mockPurchaseRepository;
        Mock<ISessionRepository> _mockSessionRepository;
        Mock<IUserRepository> _mockUserRepository;

        [SetUp]
        public void Initialize()
        {
            _mockPurchaseRepository = new Mock<IPurchaseRepository>();
            _mockSessionRepository = new Mock<ISessionRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _purchaseService = new PurchaseService(_mockPurchaseRepository.Object, _mockSessionRepository.Object, _mockUserRepository.Object, _mapper);
        }

        #region Add

        [Test]
        public void Purchase_Service_Should_Add_Purchase_Successfully()
        {
            //Arrange 
            var purchase = ObjectMother.GetDefaultPurchase();
            var purchaseCmd = ObjectMother.GetPurchaseAddCommand();
            _mockSessionRepository.Setup(r => r.GetById(It.IsAny<long>())).Returns(ObjectMother.GetDefaultSession());
            _mockPurchaseRepository.Setup(r => r.Add(It.IsAny<Purchase>())).Returns(purchase);

            //Action
            var addedPurchase = _purchaseService.Add(purchaseCmd);

            //Assert
            _mockPurchaseRepository.Verify(r => r.Add(It.IsAny<Purchase>()), Times.Once);
            addedPurchase.Id.Should().Be(purchase.Id);
        }

        [Test]
        public void Purchase_Service_Add_Should_Throw_Exception_When_Seat_Taken()
        {
            //Arrange
            var purchaseCmd = ObjectMother.GetPurchaseAddCommand();
            var stubSession = ObjectMother.GetDefaultSession();
            stubSession.TakenSeats = new Collection<Seat>
            {
                new Seat {Row = 1}
            };
            _mockSessionRepository.Setup(r => r.GetById(It.IsAny<long>())).Returns(stubSession);

            //Action
            Action act = () => _purchaseService.Add(purchaseCmd);

            //Assert
            act.Should().Throw<BusinessException>();
            _mockPurchaseRepository.Verify(r => r.Add(It.IsAny<Purchase>()), Times.Never);
        }
        #endregion
    }
}
