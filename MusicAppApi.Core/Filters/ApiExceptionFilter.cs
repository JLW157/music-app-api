using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppApi.Core.Filters
{
    public class ApiExceptionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ModelState.IsValid) return;

            Console.WriteLine(context.ToString() + " Executing");
            context.Result = new BadRequestObjectResult(context.ModelState.Select(x => x.Value.Errors.Select(x => x.ErrorMessage)).ToArray());
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine(context.ToString() + " Executed");
            }
        }
    }
}
