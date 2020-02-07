using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cinema.Domain.Exceptions;

namespace Cinema.WebAPI.Exceptions
{
    /// <summary>
    /// Objeto para retornar quando a requisição falha.
    /// </summary>
    public class ExceptionPayload
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public static ExceptionPayload New<T>(T exception) where T : Exception
        {
            int errorCode;
            if (exception is BusinessException)
                errorCode = (exception as BusinessException).ErrorCode.GetHashCode();
            else
                errorCode = ErrorCodes.Unhandled.GetHashCode();
            return new ExceptionPayload
            {
                ErrorCode = errorCode,
                ErrorMessage = exception.Message,
            };
        }
    }
}