using System;
using System.Linq;
using Cinema.Infra.ORM.Features.Users;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Users;
using FluentAssertions;
using NUnit.Framework;
using Cinema.Infra.ORM.Tests.Context;
using Effort;
using Cinema.Domain.Exceptions;

namespace Cinema.Infra.ORM.Tests.Features.Users
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private FakeDbContext _context;
        private UserRepository _repository;
        private User _user;
        private User _userBase;

        [SetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());
            _context = new FakeDbContext(connection);
            _repository = new UserRepository(_context);
            _user = ObjectMother.GetDefaultUser();

            //Seed
            _userBase = ObjectMother.GetDefaultUser();
            _context.Users.Add(_userBase);
            _context.SaveChanges();
        }

        #region ADD
        [Test]
        public void Users_Repository_Should_Add_Sucessfully()
        {
            //Action
            var user = _repository.Add(_user);
            //Assert
            user.Should().NotBeNull();
            user.Id.Should().NotBe(0);
            var expectedUser = _context.Users.Find(user.Id);
            expectedUser.Should().NotBeNull();
            expectedUser.Should().Be(user);
        }

        #endregion

        #region GET

        [Test]
        public void Users_Repository_Should_Get_All_Sucessfully()
        {
            //Action
            var users = _repository.GetAll().ToList();

            //Assert
            users.Should().NotBeNull();
            users.Should().HaveCount(_context.Users.Count());
            users.First().Should().Be(_userBase);
        }

        [Test]
        public void Users_Repository_Should_Get_By_Id_Sucessfully()
        {
            //Action
            var user = _repository.GetById(_userBase.Id);

            //Assert
            user.Should().NotBeNull();
            user.Should().Be(_userBase);
        }

        [Test]
        public void Users_Repository_Should_Get_By_Credentials_Sucessfully()
        {
            //Action
            var user = _repository.GetByCredentials(_user.Email, _user.Password);

            //Assert
            user.Should().NotBeNull();
            user.Should().Be(_userBase);
        }      
        
        [Test]
        public void Users_Repository_Should_Throw_Exception_When_Credentials_Not_Found()
        {
            //Action
            Action act = () => _repository.GetByCredentials("invalidEmail", _user.Password);

            //Assert
            act.Should().Throw<BusinessException>().WithMessage("Usuário ou senha inválidos");
        }

        [Test]
        public void Users_Repository_Verify_Email_Should_Return_False_When_Same_Id()
        {
            //Action
            var result = _repository.IsEmailAlreadyInUse(_user.Email, _user.Id);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Users_Repository_Verify_Email_Should_Return_True_When_Diferent_Id()
        {
            int newId = 0;

            //Action
            var result = _repository.IsEmailAlreadyInUse(_user.Email, newId);

            //Assert
            result.Should().BeTrue();
        }


        [Test]
        public void Users_Repository_Verify_Email_Should_Return_False()
        {
            string newEmail = "newEmail";

            //Action
            var result = _repository.IsEmailAlreadyInUse(newEmail, _user.Id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DELETE
        [Test]
        public void Users_Repository_Should_Remove_Sucessfully()
        {
            // Action
            var removed = _repository.Remove(_userBase.Id);
            // Assert
            removed.Should().BeTrue();
            _context.Users.Where(p => p.Id == _userBase.Id).FirstOrDefault().Should().BeNull();
        }

        [Test]
        public void Users_Repository_Should_Not_Remove_When_Not_Found()
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
        public void Users_Repository_Should_Update_Sucessfully()
        {
            // Arrange
            var modified = false;
            var newValue = "newName";
            _userBase.Name = newValue;
            //Action
            var act = new Action(() => { modified = _repository.Update(_userBase); });
            // Assert
            act.Should().NotThrow<Exception>();
            modified.Should().BeTrue();
        }
        #endregion
    }
}