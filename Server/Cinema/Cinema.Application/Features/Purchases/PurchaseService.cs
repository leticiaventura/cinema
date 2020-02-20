using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinema.Application.Features.Base;
using Cinema.Application.Features.Purchases.Commands;
using Cinema.Domain.Common;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Purchases.Interfaces;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;

namespace Cinema.Application.Features.Purchases
{
    public class PurchaseService : AbstractService<Purchase>, IPurchaseService
    {
        ISessionRepository _sessionRepository;
        IUserRepository _userRepository;

        public PurchaseService(IPurchaseRepository repository, ISessionRepository sessionRepository, IUserRepository userRepository, Mapper mapper) : base(repository, mapper)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public override Purchase Add(AbstractAddCommand<Purchase> command)
        {
            Purchase purchase = _mapper.Map<Purchase>(command);
            Session session = _sessionRepository.GetById(purchase.SessionId);
            purchase.Session = session;
            purchase.User = GetUserByEmail(command as PurchaseAddCommand);
            purchase.Date = DateTime.Now;
            purchase.MovieName = session.Movie.Name;
            purchase.Total = purchase.CalculateTotal();
            purchase.SessionDate = purchase.Session.Start.ToShortDateString();

            purchase.Validate();
            session.PurchasedSeats += ((PurchaseAddCommand)command).Seats;

            var newEntity = _repository.Add(purchase);
            _sessionRepository.Update(session);

            _repository.Save();
            return newEntity;
        }

        private User GetUserByEmail(PurchaseAddCommand command)
        {
            return _userRepository.GetAll()
                 .Where(x => x.Email.Equals(command.UserEmail, StringComparison.InvariantCultureIgnoreCase))
                 .FirstOrDefault();
        }
    }
}
