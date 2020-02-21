using System;
using System.Linq;
using Cinema.Infra.ORM.Features.Sessions;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Sessions;
using FluentAssertions;
using NUnit.Framework;
using Cinema.Infra.ORM.Tests.Context;
using Effort;
using Cinema.Domain.Features.Purchases;

namespace Cinema.Infra.ORM.Tests.Features.Sessions
{
    [TestFixture]
    public class SessionRepositoryTests
    {
        private FakeDbContext _context;
        private SessionRepository _repository;
        private Session _session;
        private Session _sessionBase;

        [SetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());
            _context = new FakeDbContext(connection);
            _repository = new SessionRepository(_context);
            _session = ObjectMother.GetDefaultSession();

            //Seed
            _sessionBase = ObjectMother.GetDefaultSession();
            _context.Sessions.Add(_sessionBase);
            _context.SaveChanges();
        }

        #region ADD
        [Test]
        public void Sessions_Repository_Should_Add_Sucessfully()
        {
            //Action
            var session = _repository.Add(_session);
            //Assert
            session.Should().NotBeNull();
            session.Id.Should().NotBe(0);
            var expectedSession = _context.Sessions.Find(session.Id);
            expectedSession.Should().NotBeNull();
            expectedSession.Movie.Name.Should().Be(session.Movie.Name);
        }

        #endregion

        #region GET

        [Test]
        public void Sessions_Repository_Should_Get_All_Sucessfully()
        {
            //Action
            var sessions = _repository.GetAll().ToList();

            //Assert
            sessions.Should().NotBeNull();
            sessions.Should().HaveCount(_context.Sessions.Count());
            sessions.First().Should().Be(_sessionBase);
            sessions.First().Lounge.Should().NotBeNull();
        }               

        [Test]
        public void Sessions_Repository_Should_Get_By_Id_Sucessfully()
        {
            //Action
            var session = _repository.GetById(_sessionBase.Id);

            //Assert
            session.Should().NotBeNull();
            session.Should().Be(_sessionBase);
            session.Lounge.Should().NotBeNull();
        }

        #endregion

        #region DELETE
        [Test]
        public void Sessions_Repository_Should_Remove_Sucessfully()
        {
            // Action
            _repository.Remove(_sessionBase.Id);
            var removed = _repository.Save();
            // Assert
            removed.Should().BeTrue();
            _context.Sessions.Where(p => p.Id == _sessionBase.Id).FirstOrDefault().Should().BeNull();
        }

        [Test]
        public void Sessions_Repository_Should_Not_Remove_When_Not_Found()
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
        public void Sessions_Repository_Should_Update_Sucessfully()
        {
            // Arrange
            var modified = false;
            //Action
            _sessionBase.TakenSeats.Add(new Seat { });
            _repository.Update(_sessionBase);
            var act = new Action(() => { modified = _repository.Save(); });
            // Assert
            act.Should().NotThrow<Exception>();
            modified.Should().BeTrue();
        }
        #endregion
    }
}