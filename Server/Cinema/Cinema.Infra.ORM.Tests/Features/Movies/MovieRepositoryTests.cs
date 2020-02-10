using System;
using System.Linq;
using Cinema.Infra.ORM.Features.Movies;
using Cinema.Common.Tests;
using Cinema.Domain.Features.Movies;
using FluentAssertions;
using NUnit.Framework;
using Cinema.Infra.ORM.Tests.Context;
using Effort;

namespace Cinema.Infra.ORM.Tests.Features.Movies
{
    [TestFixture]
    public class MovieRepositoryTests
    {
        private FakeDbContext _context;
        private MovieRepository _repository;
        private Movie _movie;
        private Movie _movieBase;

        [SetUp]
        public void Setup()
        {
            var connection = DbConnectionFactory.CreatePersistent(Guid.NewGuid().ToString());
            _context = new FakeDbContext(connection);
            _repository = new MovieRepository(_context);
            _movie = ObjectMother.GetDefaultMovie();

            //Seed
            _movieBase = ObjectMother.GetDefaultMovie();
            _context.Movies.Add(_movieBase);
            _context.SaveChanges();
        }

        #region ADD
        [Test]
        public void Movies_Repository_Should_Add_Sucessfully()
        {
            //Action
            var movie = _repository.Add(_movie);
            //Assert
            movie.Should().NotBeNull();
            movie.Id.Should().NotBe(0);
            var expectedMovie = _context.Movies.Find(movie.Id);
            expectedMovie.Should().NotBeNull();
            expectedMovie.Name.Should().Be(movie.Name);
        }

        #endregion

        #region GET

        [Test]
        public void Movies_Repository_Should_Get_All_Sucessfully()
        {
            //Action
            var movies = _repository.GetAll().ToList();

            //Assert
            movies.Should().NotBeNull();
            movies.Should().HaveCount(_context.Movies.Count());
            movies.First().Should().Be(_movieBase);
        }

        [Test]
        public void Movies_Repository_Should_Get_By_Id_Sucessfully()
        {
            //Action
            var movie = _repository.GetById(_movieBase.Id);

            //Assert
            movie.Should().NotBeNull();
            movie.Should().Be(_movieBase);
        }

        [Test]
        public void Movies_Repository_Verify_Name_Should_Return_False_When_Same_Id()
        {
            //Action
            var result = _repository.IsNameAlreadyInUse(_movie.Name, _movie.Id);

            //Assert
            result.Should().BeFalse();
        }

        [Test]
        public void Movies_Repository_Verify_Name_Should_Return_True_When_Diferent_Id()
        {
            int newId = 0;

            //Action
            var result = _repository.IsNameAlreadyInUse(_movie.Name, newId);

            //Assert
            result.Should().BeTrue();
        }


        [Test]
        public void Movies_Repository_Verify_Name_Should_Return_False()
        {
            string newName = "newName";

            //Action
            var result = _repository.IsNameAlreadyInUse(newName, _movie.Id);

            //Assert
            result.Should().BeFalse();
        }

        #endregion

        #region DELETE
        [Test]
        public void Movies_Repository_Should_Remove_Sucessfully()
        {
            // Action
             _repository.Remove(_movieBase.Id);
            var removed = _repository.Save();
            // Assert
            removed.Should().BeTrue();
            _context.Movies.Where(p => p.Id == _movieBase.Id).FirstOrDefault().Should().BeNull();
        }

        [Test]
        public void Movies_Repository_Should_Not_Remove_When_Not_Found()
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
        public void Movies_Repository_Should_Update_Sucessfully()
        {
            // Arrange
            var modified = false;
            var newValue = "newName";
            _movieBase.Name = newValue;
            //Action
            _repository.Update(_movieBase);
            var act = new Action(() => { modified = _repository.Save(); });
            // Assert
            act.Should().NotThrow<Exception>();
            modified.Should().BeTrue();
        }
        #endregion
    }
}