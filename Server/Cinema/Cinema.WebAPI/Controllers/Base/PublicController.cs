using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Cinema.WebAPI.Controllers.Base
{
    /// <summary>
    /// Endpoints publicos
    /// </summary>
    [RoutePrefix("api/public")]
    public class PublicController : ApiController
    {
        /// <summary>
        /// Endpoint publico para verificar se o serivdor está funcionando.
        /// </summary>
        [HttpGet]
        [Route("is-alive")]
        public IHttpActionResult IsAlive()
        {
            return Ok(true);
        }
    }
}