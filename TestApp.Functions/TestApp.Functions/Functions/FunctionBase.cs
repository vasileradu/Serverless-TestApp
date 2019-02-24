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

        protected ActionResult Ok(object obj) => new OkObjectResult(obj);

        protected ActionResult BadRequest(object obj) => new BadRequestObjectResult(obj);

        protected ActionResult Unauthorized() => new UnauthorizedResult();

        protected ActionResult NoContent() => new NoContentResult();
    }
}
