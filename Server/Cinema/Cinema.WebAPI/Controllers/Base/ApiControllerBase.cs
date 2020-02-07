using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cinema.WebAPI.Filters;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;

namespace Cinema.WebAPI.Controllers.Base
{   

    /// <summary>
    /// Classe com metodos básicos para tratar o retorno dos controllers.
    /// </summary>
    [ExceptionHandler]
    public abstract class ApiControllerBase : ApiController
    {
        protected Mapper _mapper { get; }

        protected ApiControllerBase(Mapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Retorna para o client uma requisição bem sucedida contendo o parâmetro <paramref name="data"/>. 
        /// </summary>
        /// <typeparam name="TSuccess"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IHttpActionResult HandleCallback<TSuccess>(TSuccess data)
        {
            return Ok(data);
        }

        /// <summary>
        /// Retorna para o client uma requisição bem sucedida contendo o parâmetro <paramref name="result"/> mapeado para o objeto <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        protected IHttpActionResult HandleQuery<TSource, TResult>(TSource result)
        {
            return Ok(_mapper.Map<TSource, TResult>(result));
        }

        /// <summary>
        /// Aplica as opções OData da requisição no Dataset do objeto <typeparamref name="TSource"/> e retorna para o client uma requisição bem sucedida contendo uma lista do tipo <typeparamref name="TResult"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryOptions"></param>
        /// <returns></returns>
        protected IHttpActionResult HandleQueryable<TSource, TResult>(IQueryable<TSource> query, ODataQueryOptions<TSource> queryOptions)
        {
            IQueryable<TSource> odataQuery = queryOptions.ApplyTo(query).Cast<TSource>();
            IQueryable<TResult> dataQuery = odataQuery.ToList().AsQueryable().ProjectTo<TResult>(_mapper.ConfigurationProvider);

            var pageResult = new PageResult<TResult>(dataQuery, Request.ODataProperties().NextLink, Request.ODataProperties().TotalCount);
            return Ok(pageResult);
        }

        /// <summary>
        /// Retorna uma requisição com falha contendo uma lista das validações que falharam.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validationFailure"></param>
        /// <returns></returns>
        protected IHttpActionResult HandleValidationFailure<T>(IList<T> validationFailure) where T : ValidationFailure
        {
            return Content(HttpStatusCode.BadRequest, validationFailure);
        }
    }
}