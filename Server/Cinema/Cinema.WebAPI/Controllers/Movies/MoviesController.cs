using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Cinema.Application.Features.Movies.Commands;
using Cinema.Application.Features.Movies.Queries;
using Cinema.Application.Features.Movies.ViewModels;
using Cinema.Domain.Features.Movies;
using Cinema.Domain.Features.Movies.Interfaces;
using Cinema.WebAPI.Controllers.Base;
using Cinema.WebAPI.Filters;
using Microsoft.AspNet.OData.Query;

namespace Cinema.WebAPI.Controllers.Movies
{
    [Authorize(Roles = "Admin")]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/movies")]
    public class MoviesController : ApiControllerBase
    {
        private readonly IMovieService _service;
        public MoviesController(Mapper mapper, IMovieService service) : base(mapper)
        {
            _service = service;
        }

        #region HttpGet
        [HttpGet]
        [ODataQueryOptionsValidate]
        public IHttpActionResult Get(ODataQueryOptions<Movie> queryOptions)
        {
            return HandleQueryable<Movie, MovieGridViewModel>(_service.GetAll(), queryOptions);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            return HandleQuery<Movie, MovieViewModel>(_service.GetById(id));
        }
        #endregion HttpGet

        #region HttpPost
        [HttpPost]
        public IHttpActionResult Post(MovieAddCommand MovieCmd)
        {
            var validator = MovieCmd.Validate(_service);
            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Add(MovieCmd));
        }

        [HttpPost]
        [Route("name")]
        public IHttpActionResult CheckName(MovieCheckNameQuery query)
        {
            return HandleCallback(_service.IsNameAlreadyInUse(query.Name, query.Id));
        }
        #endregion HttpPost

        #region HttpPatch
        [HttpPatch]
        public IHttpActionResult Update(MovieUpdateCommand command)
        {
            var validator = command.Validate(_service);

            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Update(command));
        }
        #endregion HttpPatch

        #region HttpDelete
        [HttpDelete]
        public IHttpActionResult Delete([FromBody] long id)
        {
            return HandleCallback(_service.Remove(id));
        }
        #endregion HttpDelete
    }
}