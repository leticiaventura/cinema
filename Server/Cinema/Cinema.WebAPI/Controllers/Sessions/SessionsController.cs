using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    [Authorize(Roles = "Admin, Employee, Customer")]
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
            return HandleQueryable<Session, SessionGridViewModel>(_service.GetAll(), queryOptions);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            return HandleCallback(_service.GetById(id));
        }

        [HttpGet]
        [Route("by-date")]
        public IHttpActionResult GetByDate()
        {            
            if (Request.RequestUri.ParseQueryString()["date"] != null)
            {
                DateTime date = Convert.ToDateTime(Request.RequestUri.ParseQueryString()["date"]).ToLocalTime();
                List<Session> sessions = _service.GetAll().Where(x => x.Start.Year == date.Year && x.Start.Month == date.Month && x.Start.Day == date.Day).ToList();
                return Ok(sessions);
            }
            return Ok(new List<SessionGridViewModel>());
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