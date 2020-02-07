using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Cinema.WebAPI.Controllers.Base;
using Cinema.WebAPI.Filters;
using Cinema.Application.Features.Users.ViewModels;
using Cinema.Domain.Features.Users;
using Cinema.Domain.Features.Users.Interfaces;
using Microsoft.AspNet.OData.Query;
using Cinema.Application.Features.Users.Commands;
using System.Web.Http.Cors;
using Cinema.Application.Features.Users.Queries;
using System.Security.Claims;

namespace Cinema.WebAPI.Controllers.Users
{
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        private readonly IUserService _service;
        public UsersController(Mapper mapper, IUserService service) : base(mapper)
        {
            _service = service;
        }

        #region HttpGet
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ODataQueryOptionsValidate]
        public IHttpActionResult Get(ODataQueryOptions<User> queryOptions)
        {
                var queryString = Request.GetQueryNameValuePairs()
                                    .Where(x => x.Key.Equals("size"))
                                    .FirstOrDefault();

            var query = default(IQueryable<User>);
            int size = 0;
            if (queryString.Key != null && int.TryParse(queryString.Value, out size))
            {
                query = _service.GetAll(size);
            }
            else
                query = _service.GetAll();

            return HandleQueryable<User, UserViewModel>(query, queryOptions);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            return HandleQuery<User, UserViewModel>(_service.GetById(id));
        }

        [HttpGet]
        [Authorize]
        [Route("role")]
        public IHttpActionResult IdentifyRole()
        {
            var identity = (ClaimsIdentity)User.Identity;
            string claim = identity.Claims.First().Value;

            switch (claim.ToLower())
            {
                case "admin":
                    return Ok(1);

                case "employee":
                    return Ok(2);

                case "customer":
                    return Ok(3);

                default:
                    return Ok(0);
            };
        }
        #endregion HttpGet

        #region HttpPost
        [HttpPost]
        public IHttpActionResult Post(UserAddCommand UserCmd)
        {
            var validator = UserCmd.Validate(_service);
            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Add(UserCmd));
        }

        [HttpPost]
        [Route("email")]
        public IHttpActionResult CheckEmail(LoungeCheckNameQuery query)
        {
            return HandleCallback(_service.IsEmailAlreadyInUse(query.Email, query.Id));
        }
        #endregion HttpPost

        #region HttpPatch
        [HttpPatch]
        public IHttpActionResult Update(UserUpdateCommand command)
        {
            var validator = command.Validate(_service);

            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            return HandleCallback(_service.Update(command));
        }
        #endregion HttpPatch

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