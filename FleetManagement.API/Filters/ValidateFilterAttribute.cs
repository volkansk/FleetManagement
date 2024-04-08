using Microsoft.AspNetCore.Mvc.Filters;

namespace FleetManagement.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                throw new ApplicationException(string.Join(',', errors));
            }
        }
    }
}
