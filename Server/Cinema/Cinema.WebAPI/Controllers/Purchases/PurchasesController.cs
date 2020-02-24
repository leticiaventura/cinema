using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using Cinema.Application.Features.Movies.ViewModels;
using Cinema.Application.Features.Purchases.Commands;
using Cinema.Application.Features.Purchases.ViewModels;
using Cinema.Domain.Features.Purchases;
using Cinema.Domain.Features.Purchases.Interfaces;
using Cinema.WebAPI.Controllers.Base;
using Cinema.WebAPI.Filters;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;

namespace Cinema.WebAPI.Controllers.Purchases
{
    [Authorize(Roles = "Admin, Employee, Customer")]
    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/purchases")]
    public class PurchasesController : ApiControllerBase
    {
        private readonly IPurchaseService _service;
        public PurchasesController(Mapper mapper, IPurchaseService service) : base(mapper)
        {
            _service = service;
        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public IHttpActionResult Post(PurchaseAddCommand purchaseCmd)
        {
            var validator = purchaseCmd.Validate(_service);
            if (!validator.IsValid)
                return HandleValidationFailure(validator.Errors);

            var identity = (ClaimsIdentity)User.Identity;
            string claim = identity.Claims.ToArray()[0].Value;

            purchaseCmd.UserEmail = claim;

            return HandleCallback(_service.Add(purchaseCmd));
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IHttpActionResult Get(ODataQueryOptions<Purchase> queryOptions)
        {
            string user = ((ClaimsIdentity)User.Identity).Claims.ToArray()[0].Value;
            string filter = queryOptions.Filter == null ? "" : queryOptions.Filter.RawValue;

            var all = _service.GetAll()
                .Where(x => x.User.Email.Equals(user))
                .Where(x => x.MovieName.StartsWith(filter)
                    || x.SessionDate.StartsWith(filter)
                    || x.Session.Lounge.Name.StartsWith(filter));

            int count = all.Count();

            all = all.OrderBy(x => x.Id)
                .Skip(queryOptions.Skip.Value)
                .Take(queryOptions.Top.Value);

            var result = _mapper.Map<IList<PurchaseGridViewModel>>(all.ToList());

            var pageResult = new PageResult<PurchaseGridViewModel>(result, null, count);
            return Ok(pageResult);
        }

        [HttpGet]
        [Authorize(Roles = "Employee")]
        [Route("check-in")]
        public IHttpActionResult FindTickets(ODataQueryOptions<Purchase> queryOptions)
        {
            var allTickets = _service.GetAll();
            allTickets = FilterByDate(allTickets);
            allTickets = FilterByName(allTickets);
            int count = allTickets.Count();

            allTickets = allTickets.OrderBy(x => x.Id)
                .Skip(queryOptions.Skip.Value)
                .Take(queryOptions.Top.Value);

            var result = _mapper.Map<IList<PurchasedTicketGridViewModel>>(allTickets.ToList());

            var pageResult = new PageResult<PurchasedTicketGridViewModel>(result, null, count);
            return Ok(pageResult);
        }

        private IQueryable<Purchase> FilterByDate(IQueryable<Purchase> allTickets)
        {
            if (!string.IsNullOrEmpty(Request.RequestUri.ParseQueryString()["$filterDate"]))
            {
                string date = Convert.ToDateTime(Request.RequestUri.ParseQueryString()["$filterDate"]).ToLocalTime().ToShortDateString();
                allTickets = allTickets.Where(x => x.SessionDate.Equals(date));
            }
            return allTickets;
        }

        private IQueryable<Purchase> FilterByName(IQueryable<Purchase> allTickets)
        {
            if (!string.IsNullOrEmpty(Request.RequestUri.ParseQueryString()["$filterName"]))
            {
                string name = Request.RequestUri.ParseQueryString()["$filterName"];
                allTickets = allTickets.Where(x => x.User.Name.Contains(name));
            }
            return allTickets;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("report/movie")]
        public IHttpActionResult GetMovieResport(ODataQueryOptions<Purchase> queryOptions)
        {
            string filter = queryOptions.Filter == null ? "" : queryOptions.Filter.RawValue;

            var allPurchases = _service.GetAll();
            var movies = allPurchases.Select(x => x.Session.Movie).Distinct().Where(m => m.Name.Contains(filter)).ToList();

            foreach (var movie in movies)
            {
                movie.Revenue = allPurchases.Where(x => x.Session.Movie.Id == movie.Id).Select(x => x.Total).Sum();
            }

            int count = movies.Count();

            movies = movies.OrderByDescending(x => x.Revenue)
                .Skip(queryOptions.Skip.Value)
                .Take(queryOptions.Top.Value).ToList();

            var result = _mapper.Map<IList<MovieReportViewModel>>(movies);

            var pageResult = new PageResult<MovieReportViewModel>(result, null, count);
            return Ok(pageResult);
        }
    }
}