using System;
using System.Linq;
using Cinema.Infra.ORM.Features.Purchases;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Purchases;
using FluentAssertions;
using NUnit.Framework;
using Cinema.Infra.ORM.Tests.Context;
using Effort;

namespace Cinema.Infra.ORM.Tests.Features.Purchases
{
    [TestFixture]
    public class PurchaseRepositoryTests
    {
        private FakeDbContext _context;
        private PurchaseRepository _repository;
        private Purchase _purchase;
        private Purchase _purchaseBase;

        [SetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());
            _context = new FakeDbContext(connection);
            _repository = new PurchaseRepository(_context);
            _purchase = ObjectMother.GetDefaultPurchase();

            //Seed
            _purchaseBase = ObjectMother.GetDefaultPurchase();
            _context.Purchases.Add(_purchaseBase);
            _context.SaveChanges();
        }

        #region ADD
        [Test]
        public void Purchases_Repository_Should_Add_Sucessfully()
        {
            //Action
            var purchase = _repository.Add(_purchase);
            //Assert
            purchase.Should().NotBeNull();
            purchase.Id.Should().NotBe(0);
            var expectedPurchase = _context.Purchases.Find(purchase.Id);
            expectedPurchase.Should().NotBeNull();
        }

        #endregion

        #region GET
        [Test]
        public void Purchases_Repository_Should_Get_All_Sucessfully()
        {
            //Action
            var purchases = _repository.GetAll().ToList();

            //Assert
            purchases.Should().NotBeNull();
            purchases.Should().HaveCount(1);
            purchases.First().Should().Be(_purchaseBase);
            purchases.First().User.Should().NotBeNull();
            purchases.First().Session.Should().NotBeNull();
        }

        #endregion
    }
}