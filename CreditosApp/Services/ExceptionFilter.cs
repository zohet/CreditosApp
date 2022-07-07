using CuentasAhorroApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace CuentasAhorroApp.Services
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Response result = new Response();
            result.IsSuccess = false;

            if (context.Exception is Exception)
            {
                var exception = context.Exception;
                result.Message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;
                result.Result = exception;
            }
            else
                result.Message = "ExceptionManagerFilter";
            context.Result = new JsonResult(result);
        }
    }
}
