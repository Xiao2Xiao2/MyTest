using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTest.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Http401()
        {
            return View();
        }
        public ActionResult Http403()
        {
            return View();
        }
        public ActionResult Http500()
        {
            return View();
        }
    }
}
