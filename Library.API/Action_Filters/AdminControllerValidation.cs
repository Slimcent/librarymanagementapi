using Library.LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Action_Filters
{
    public class AdminControllerValidation : ActionFilterAttribute
    {
        private readonly ILoggerManager _logger;
        public AdminControllerValidation(ILoggerManager logger)
        {
            _logger = logger;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["contoller"];
            if (!context.ModelState.IsValid)
            {
                _logger.LogError($"Invalid model state for the object. Controller: {controller}, action: { action}");
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
    }
}