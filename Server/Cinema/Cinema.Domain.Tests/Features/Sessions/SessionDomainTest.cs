using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cinema.Common.Tests;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Features.Sessions;
using FluentAssertions;
using NUnit.Framework;

namespace Cinema.Domain.Tests.Features.Sessions
{
    [TestFixture]
    class SessionServiceTest
    {
        [Test]
        public void Should_Verify_Session_Successfully()
        {
            //Arrange 
            int movieLengthInMinutes = 90;
            var session = ObjectMother.GetDefaultSession();
            session.Movie.Length = movieLengthInMinutes;
            session.Start = new DateTime(2020, 01, 01, 16, 00, 00);
            session.End = session.Start.AddMinutes(session.Movie.Length);

            //Action
            Action act = () => session.Validate();

            //Assert
            act.Should().NotThrow<BusinessException>();
            session.End.Should().Be(new DateTime(2020, 01, 01, 17, 30, 00));
        }

        [Test]
        public void Should_Not_Validate_Session_With_EndTime_Error()
        {
            //Arrange 
            var session = ObjectMother.GetDefaultSession();
            session.End = DateTime.Now;

            //Action
            Action act = () => session.Validate();

            //Assert
            act.Should().Throw<BusinessException>().WithMessage("O horário de fim deve ser igual ao início + duração do filme.");
        }


        [Test]
        public void Should_Not_Validate_Session_With_Seats_Error()
        {
            //Arrange 
            int movieLengthInMinutes = 90;
            var session = ObjectMother.GetDefaultSession();
            session.Movie.Length = movieLengthInMinutes;
            session.Start = new DateTime(2020, 01, 01, 16, 00, 00);
            session.End = session.Start.AddMinutes(session.Movie.Length);

            session.PurchasedSeats = 999;
            session.Lounge.Seats = 20;

            //Action
            Action act = () => session.Validate();

            //Assert
            act.Should().Throw<BusinessException>().WithMessage("A quantidade de lugares compradas não pode ser maior que a quantidade de lugares disponíveis na sala.");
        }
    }
}