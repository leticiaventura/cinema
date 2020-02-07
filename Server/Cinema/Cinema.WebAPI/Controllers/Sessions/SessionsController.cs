using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Cinema.Application.Features.Lounges.ViewModels;
using Cinema.Application.Features.Sessions.Commands;
using Cinema.Application.Features.Sessions.Queries;
using Cinema.Application.Features.Sessions.ViewModels;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Sessions;
using Cinema.Domain.Features.Sessions.Interfaces;
using Cinema.WebAPI.Controllers.Base;
using Cinema.WebAPI.Filters;
using Microsoft.AspNet.OData.Query;

namespace Cinema.WebAPI.Controllers.Sessions
{
    [Authorize(Roles = "Admin, Employee")]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/sessions")]
    public class SessionsController : ApiControllerBase
    {
        private readonly ISessionService _service;
        public SessionsController(Mapper mapper, ISessionService service) : base(mapper)
        {
            _service = service;
        }

        #region HttpGet
        [HttpGet]
        [ODataQueryOptionsValidate]
        public IHttpActionResult Get(ODataQueryOptions<Session> queryOptions)
        {
            var queryString = Request.GetQueryNameValuePairs()
                                    .Where(x => x.Key.Equals("size"))
                                    .FirstOrDefault();

            var query = default(IQueryable<Session>);
            int size = 0;
            if (queryString.Key != null && int.TryParse(queryString.Value, out size))
            {
                query = _service.GetAll(size);
            }
            else
                query = _service.GetAll();

            return HandleQueryable<Session, SessionGridViewModel>(query, queryOptions);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            return HandleQuery<Session, SessionViewModel>(_service.GetById(id));
        }
        #endregion HttpGet

        #region HttpPost
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Post(SessionAddCommand SessionCmd)
        {
            var validator = SessionCmd.Validate(_service);
            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Add(SessionCmd));
        }

        [HttpPost]
        [Route("{availablelounges}")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAvailableLounges(SessionGetAvailableLoungesQuery query)
        {
            var result = _service.GetAvailableLounges(_mapper.Map<Session>(query));
            return Ok(result);
        }
        #endregion HttpPost

        #region HttpDelete
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete([FromBody] long id)
        {
            return HandleCallback(_service.Remove(id));
        }
        #endregion HttpDelete
    }
}