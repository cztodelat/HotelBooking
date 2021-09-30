using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["ErrorMessage"] = "Sorry, the resource you requested could not be found";
                    break;
            }

            return View("NotFound");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            //Get exception details
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //TODO Log this details

            //ViewData["ExceptionPath"] = exceptionDetails.Path;
            //ViewData["ExceptionDetails"] = exceptionDetails.Error.Message;
            //ViewData["Stacktrace"] = exceptionDetails.Error.StackTrace;

            return View("Error");
        }
    }
}
