﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTest.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        public ActionResult Default()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult WorkTable()
        {
            return View();
        }
    }
}
