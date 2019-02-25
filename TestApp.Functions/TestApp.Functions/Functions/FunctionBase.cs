using Microsoft.AspNetCore.Mvc;
using TestApp.Functions.Helpers;

namespace TestApp.Functions.Functions
{
    public abstract class FunctionBase
    {
        protected ModelState ModelState { get; }

        protected System.IServiceProvider Container { get; }

        protected FunctionBase()
        {
            this.ModelState = new ModelState();
            this.Container = new Startup().Configure().Build();
        }

        protected ActionResult Ok(object value) => new OkObjectResult(value);

        protected ActionResult BadRequest(object value) => new BadRequestObjectResult(value);

        protected ActionResult Unauthorized() => new UnauthorizedResult();

        protected ActionResult NoContent() => new NoContentResult();

        protected ActionResult NotFound(object value) => new NotFoundObjectResult(value);
    }
}
