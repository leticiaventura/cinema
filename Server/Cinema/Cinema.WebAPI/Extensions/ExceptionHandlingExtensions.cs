using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Cinema.WebAPI.Exceptions;
using Cinema.Domain.Exceptions;

namespace Cinema.WebAPI.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        /// <summary>
        /// Estrutura uma resposta http com a exceção disparada no <paramref name="context"/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HttpResponseMessage HandleExecutedContextException(this HttpActionExecutedContext context)
        {
            var exceptionPayload = ExceptionPayload.New(context.Exception);
            return context.Exception is BusinessException ?
                context.Request.CreateResponse(HttpStatusCode.BadRequest, exceptionPayload) :
                context.Request.CreateResponse(HttpStatusCode.InternalServerError, exceptionPayload);
        }
    }
}