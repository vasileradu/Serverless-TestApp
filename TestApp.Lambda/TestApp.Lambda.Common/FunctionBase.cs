using System;
using TestApp.Lambda.Common.Models;

namespace TestApp.Lambda.Common
{
    public abstract class FunctionBase
    {
        protected ModelState ModelState { get; }
        protected IServiceProvider Container { get; }

        protected FunctionBase()
        {
            this.ModelState = new ModelState();
            this.Container = new Startup().Configure().Build();
        }

        protected FunctionResult Ok(object value) => new FunctionResult(200, "Ok", value);

        protected FunctionResult BadRequest(object value) => new FunctionResult(400, "BadRequest", value);

        protected FunctionResult Unauthorized() => new FunctionResult(401, "Unauthorized");

        protected FunctionResult NoContent() => new FunctionResult(204, "NoContent");

        protected FunctionResult NotFound(object value) => new FunctionResult(404, "NotFound", value);
    }
}
