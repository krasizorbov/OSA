namespace OSA.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = this.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    this.ViewBag.ErrorMessage = "Sorry the resource you requested could not be found!";
                    this.ViewBag.Path = statusCodeResult.OriginalPath;
                    this.ViewBag.QS = statusCodeResult.OriginalQueryString;
                    break;
            }

            return this.View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exeptionDetails = this.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            this.ViewBag.ExeptionPath = exeptionDetails.Path;
            this.ViewBag.ExeptionMessage = exeptionDetails.Error.Message;
            return this.View("Error");
        }
    }
}