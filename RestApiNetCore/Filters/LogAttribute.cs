using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace RestApiNetCore.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        public LogAttribute()
        {
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Trace.WriteLine(string.Format("Action Method {0} executing in {1} ", 
                                           actionExecutedContext.ActionContext.ActionDescriptor.ActionName, 
                                           actionExecutedContext.ActionContext.ActionDescriptor.ActionName));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Trace.WriteLine(string.Format("Action Method {0} executing at {1}", 
                                          actionContext.ActionDescriptor.ActionName,
                                          DateTime.Now.ToShortDateString()
                ), "Web Api Logs");
        }
    }
}
