﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
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

            var all = _service.GetAll().Where(x => x.User.Email.Equals(user));
            int count = all.Count();
            string filter = queryOptions.Filter == null ? "" : queryOptions.Filter.RawValue;
            all = all.OrderBy(x => x.Id)
                .Skip(queryOptions.Skip.Value)
                .Take(queryOptions.Top.Value)
                .Where(x => x.MovieName.StartsWith(filter) 
                    || x.SessionDate.StartsWith(filter) 
                    || x.Session.Lounge.Name.StartsWith(filter));

            var result = _mapper.Map<IList<PurchaseGridViewModel>>(all.ToList());

            var pageResult = new PageResult<PurchaseGridViewModel>(result, null, count);
            return Ok(pageResult);
        }
    }
}