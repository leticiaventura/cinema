using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cinema.Domain.Common;
using Cinema.Infra.ORM.Base;

namespace Cinema.Application.Features.Base
{
    /// <summary>
    /// Classe abstrata para ser utilizada pelos serviços. Contém operações básicas de CRUD.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractService<T> : IService<T> where T : Entity
    {
        protected IRepository<T> _repository;
        protected Mapper _mapper;

        public AbstractService(IRepository<T> repository, Mapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual T Add(AbstractAddCommand<T> command)
        {
            T entity = _mapper.Map<T>(command);
            entity.Validate();
            var newEntity = _repository.Add(entity);
            _repository.Save();
            return newEntity;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public virtual T GetById(long id)
        {
            return _repository.GetById(id);
        }

        public virtual bool Remove(long id)
        {
            _repository.Remove(id);
            return _repository.Save();
        }

        public virtual bool Update(AbstractUpdateCommand<T> command)
        {
            T previousEntity = GetById(command.Id);
            if (previousEntity != null)
            {
                _mapper.Map(command, previousEntity);
                previousEntity.Validate();
                 _repository.Update(previousEntity);
            }
            return _repository.Save();
        }
    }
}
