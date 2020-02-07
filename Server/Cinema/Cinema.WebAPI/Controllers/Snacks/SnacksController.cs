using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Cinema.Application.Features.Snacks.Commands;
using Cinema.Application.Features.Snacks.Queries;
using Cinema.Application.Features.Snacks.ViewModels;
using Cinema.Domain.Features.Snacks;
using Cinema.Domain.Features.Snacks.Interfaces;
using Cinema.WebAPI.Controllers.Base;
using Cinema.WebAPI.Filters;
using Microsoft.AspNet.OData.Query;

namespace Cinema.WebAPI.Controllers.Snacks
{
    [Authorize(Roles = "Admin")]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/snacks")]
    public class SnacksController : ApiControllerBase
    {
        private readonly ISnackService _service;
        public SnacksController(Mapper mapper, ISnackService service) : base(mapper)
        {
            _service = service;
        }

        #region HttpGet
        [HttpGet]
        [ODataQueryOptionsValidate]
        public IHttpActionResult Get(ODataQueryOptions<Snack> queryOptions)
        {
            var queryString = Request.GetQueryNameValuePairs()
                                    .Where(x => x.Key.Equals("size"))
                                    .FirstOrDefault();

            var query = default(IQueryable<Snack>);
            int size = 0;
            if (queryString.Key != null && int.TryParse(queryString.Value, out size))
            {
                query = _service.GetAll(size);
            }
            else
                query = _service.GetAll();

            return HandleQueryable<Snack, SnackGridViewModel>(query, queryOptions);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            return HandleQuery<Snack, SnackViewModel>(_service.GetById(id));
        }
        #endregion HttpGet

        #region HttpPost
        [HttpPost]
        public IHttpActionResult Post(SnackAddCommand SnackCmd)
        {
            //var validator = 
                SnackCmd.Validate(_service);
            //if (!validator.IsValid)
            //    return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Add(SnackCmd));
        }

        [HttpPost]
        [Route("name")]
        public IHttpActionResult CheckEmail(SnackCheckNameQuery query)
        {
            return HandleCallback(_service.IsNameAlreadyInUse(query.Name, query.Id));
        }
        #endregion HttpPost

        #region HttpPatch
        [HttpPatch]
        public IHttpActionResult Update(SnackUpdateCommand command)
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