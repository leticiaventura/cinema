using System;
using System.Linq;
using Cinema.Infra.ORM.Features.Snacks;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Snacks;
using FluentAssertions;
using NUnit.Framework;
using Cinema.Infra.ORM.Tests.Context;
using Effort;

namespace Cinema.Infra.ORM.Tests.Features.Snacks
{
    [TestFixture]
    public class SnackRepositoryTests
    {
        private FakeDbContext _context;
        private SnackRepository _repository;
        private Snack _snack;
        private Snack _snackBase;

        [SetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());
            _context = new FakeDbContext(connection);
            _repository = new SnackRepository(_context);
            _snack = ObjectMother.GetDefaultSnack();

            //Seed
            _snackBase = ObjectMother.GetDefaultSnack();
            _context.Snacks.Add(_snackBase);
            _context.SaveChanges();
        }

        #region ADD
        [Test]
        public void Snacks_Repository_Should_Add_Sucessfully()
        {
            //Action
            var snack = _repository.Add(_snack);
            //Assert
            snack.Should().NotBeNull();
            snack.Id.Should().NotBe(0);
            var expectedSnack = _context.Snacks.Find(snack.Id);
            expectedSnack.Should().NotBeNull();
            expectedSnack.Name.Should().Be(snack.Name);
        }

        #endregion

        #region GET

        [Test]
        public void Snacks_Repository_Should_Get_All_Sucessfully()
        {
            //Action
            var snacks = _repository.GetAll().ToList();

            //Assert
            snacks.Should().NotBeNull();
            snacks.Should().HaveCount(_context.Snacks.Count());
            snacks.First().Should().Be(_snackBase);
        }

        [Test]
        public void Snacks_Repository_Should_Get_By_Id_Sucessfully()
        {
            //Action
            var snack = _repository.GetById(_snackBase.Id);

            //Assert
            snack.Should().NotBeNull();
            snack.Should().Be(_snackBase);
        }

        [Test]
        public void Snacks_Repository_Verify_Name_Should_Return_False_When_Same_Id()
        {
            //Action
            var result = _repository.IsNameAlreadyInUse(_snack.Name, _snack.Id);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Snacks_Repository_Verify_Name_Should_Return_True_When_Diferent_Id()
        {
            int newId = 0;

            //Action
            var result = _repository.IsNameAlreadyInUse(_snack.Name, newId);

            //Assert
            result.Should().BeTrue();
        }


        [Test]
        public void Snacks_Repository_Verify_Name_Should_Return_False()
        {
            string newName = "newName";

            //Action
            var result = _repository.IsNameAlreadyInUse(newName, _snack.Id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DELETE
        [Test]
        public void Snacks_Repository_Should_Remove_Sucessfully()
        {
            // Action
            _repository.Remove(_snackBase.Id);
            var removed = _repository.Save();
            // Assert
            removed.Should().BeTrue();
            _context.Snacks.Where(p => p.Id == _snackBase.Id).FirstOrDefault().Should().BeNull();
        }

        [Test]
        public void Snacks_Repository_Should_Not_Remove_When_Not_Found()
        {
            // Arrange
            var idInvalid = 10;
            // Action
            _repository.Remove(idInvalid);
            var result = _repository.Save();
            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region UPDATE

        [Test]
        public void Snacks_Repository_Should_Update_Sucessfully()
        {
            // Arrange
            var modified = false;
            var newValue = "newName";
            _snackBase.Name = newValue;
            //Action
            _repository.Update(_snackBase);
            var act = new Action(() => { modified = _repository.Save(); });
            // Assert
            act.Should().NotThrow<Exception>();
            modified.Should().BeTrue();
        }
        #endregion
    }
}