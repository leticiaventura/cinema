using System;
using System.Linq;
using Cinema.Infra.ORM.Features.Lounges;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Lounges;
using FluentAssertions;
using NUnit.Framework;
using Cinema.Infra.ORM.Tests.Context;
using Effort;

namespace Cinema.Infra.ORM.Tests.Features.Lounges
{
    [TestFixture]
    public class LoungeRepositoryTests
    {
        private FakeDbContext _context;
        private LoungeRepository _repository;
        private Lounge _lounge;
        private Lounge _loungeBase;

        [SetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());
            _context = new FakeDbContext(connection);
            _repository = new LoungeRepository(_context);
            _lounge = ObjectMother.GetDefaultLounge();

            //Seed
            _loungeBase = ObjectMother.GetDefaultLounge();
            _context.Lounges.Add(_loungeBase);
            _context.SaveChanges();
        }

        #region ADD
        [Test]
        public void Lounges_Repository_Should_Add_Sucessfully()
        {
            //Action
            var lounge = _repository.Add(_lounge);
            //Assert
            lounge.Should().NotBeNull();
            lounge.Id.Should().NotBe(0);
            var expectedLounge = _context.Lounges.Find(lounge.Id);
            expectedLounge.Should().NotBeNull();
            expectedLounge.Should().Be(lounge);
        }

        #endregion

        #region GET

        [Test]
        public void Lounges_Repository_Should_Get_All_Sucessfully()
        {
            //Action
            var lounges = _repository.GetAll().ToList();

            //Assert
            lounges.Should().NotBeNull();
            lounges.Should().HaveCount(_context.Lounges.Count());
            lounges.First().Should().Be(_loungeBase);
        }

        [Test]
        public void Lounges_Repository_Should_Get_By_Id_Sucessfully()
        {
            //Action
            var lounge = _repository.GetById(_loungeBase.Id);

            //Assert
            lounge.Should().NotBeNull();
            lounge.Should().Be(_loungeBase);
        }

        [Test]
        public void Lounges_Repository_Verify_Name_Should_Return_False_When_Same_Id()
        {
            //Action
            var result = _repository.IsNameAlreadyInUse(_lounge.Name, _lounge.Id);

            //Assert
            result.Should().BeFalse();
        } 
        
        [Test]
        public void Lounges_Repository_Verify_Name_Should_Return_True_When_Diferent_Id()
        {
            int newId = 0;

            //Action
            var result = _repository.IsNameAlreadyInUse(_lounge.Name, newId);

            //Assert
            result.Should().BeTrue();
        }


        [Test]
        public void Lounges_Repository_Verify_Name_Should_Return_False()
        {
            string newName = "newName";

            //Action
            var result = _repository.IsNameAlreadyInUse(newName, _lounge.Id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DELETE
        [Test]
        public void Lounges_Repository_Should_Remove_Sucessfully()
        {
            // Action
            var removed = _repository.Remove(_loungeBase.Id);
            // Assert
            removed.Should().BeTrue();
            _context.Lounges.Where(p => p.Id == _loungeBase.Id).FirstOrDefault().Should().BeNull();
        }

        [Test]
        public void Lounges_Repository_Should_Not_Remove_When_Not_Found()
        {
            // Arrange
            var idInvalid = 10;
            // Action
            var result = _repository.Remove(idInvalid);
            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region UPDATE

        [Test]
        public void Lounges_Repository_Should_Update_Sucessfully()
        {
            // Arrange
            var modified = false;
            var newValue = 66;
            _loungeBase.Seats = newValue;
            //Action
            var act = new Action(() => { modified = _repository.Update(_loungeBase); });
            // Assert
            act.Should().NotThrow<Exception>();
            modified.Should().BeTrue();
        }
        #endregion
    }
}