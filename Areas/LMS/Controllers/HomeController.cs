using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPayroll.Areas.LMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: LMS/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}