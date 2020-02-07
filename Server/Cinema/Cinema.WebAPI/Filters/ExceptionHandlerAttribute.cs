using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using Cinema.WebAPI.Extensions;

namespace Cinema.WebAPI.Filters
{
    /// <summary>
    /// Filtro para tratar exceções
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Retorna uma resposta http quando ocorre uma exceção.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = context.HandleExecutedContextException();
        }
    }
}