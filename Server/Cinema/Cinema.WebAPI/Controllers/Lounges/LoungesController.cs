using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Cinema.Application.Features.Lounges.Commands;
using Cinema.Application.Features.Lounges.Queries;
using Cinema.Application.Features.Lounges.ViewModels;
using Cinema.Domain.Features.Lounges;
using Cinema.Domain.Features.Lounges.Interfaces;
using Cinema.WebAPI.Controllers.Base;
using Cinema.WebAPI.Filters;
using Microsoft.AspNet.OData.Query;

namespace Cinema.WebAPI.Controllers.Lounges
{
    [Authorize(Roles = "Admin")]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/lounges")]
    public class LoungesController : ApiControllerBase
    {
        private readonly ILoungeService _service;
        public LoungesController(Mapper mapper, ILoungeService service) : base(mapper)
        {
            _service = service;
        }

        #region HttpGet
        [HttpGet]
        [ODataQueryOptionsValidate]
        public IHttpActionResult Get(ODataQueryOptions<Lounge> queryOptions)
        {
            var queryString = Request.GetQueryNameValuePairs()
                                    .Where(x => x.Key.Equals("size"))
                                    .FirstOrDefault();

            var query = default(IQueryable<Lounge>);
            int size = 0;
            if (queryString.Key != null && int.TryParse(queryString.Value, out size))
            {
                query = _service.GetAll(size);
            }
            else
                query = _service.GetAll();

            return HandleQueryable<Lounge, LoungeViewModel>(query, queryOptions);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            return HandleQuery<Lounge, LoungeViewModel>(_service.GetById(id));
        }
        #endregion HttpGet

        #region HttpPost
        [HttpPost]
        public IHttpActionResult Post(LoungeAddCommand LoungeCmd)
        {
            var validator = LoungeCmd.Validate(_service);
            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Add(LoungeCmd));
        }

        [HttpPost]
        [Route("name")]
        public IHttpActionResult CheckEmail(MovieCheckNameQuery query)
        {
            return HandleCallback(_service.IsNameAlreadyInUse(query.Name, query.Id));
        }
        #endregion HttpPost

        #region HttpPatch
        [HttpPatch]
        public IHttpActionResult Update(LoungeUpdateCommand command)
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