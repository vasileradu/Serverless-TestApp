using Microsoft.AspNetCore.Mvc;
using TestApp.Functions.Helpers;

namespace TestApp.Functions.Functions
{
    public abstract class FunctionBase
    {
        protected ModelState ModelState { get; private set; }

        protected readonly System.IServiceProvider _container;

        protected FunctionBase()
        {
            this.ModelState = new ModelState();
            this._container = new Startup().Configure().Build();
        }

        protected ActionResult Ok(object obj) => new OkObjectResult(obj);

        protected ActionResult BadRequest(object obj) => new BadRequestObjectResult(obj);

        protected ActionResult Unauthorized() => new UnauthorizedResult();
    }
}
