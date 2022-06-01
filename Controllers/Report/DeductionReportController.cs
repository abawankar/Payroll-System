using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Report
{
    public class DeductionReportController : Controller
    {
        // GET: DeductionReport
        public ActionResult Index()
        {
            return View();
        }

        #region ---- Monthly Deduction Report ---

        public ActionResult MonthlyDednReport()
        {
            return View();
        }
       
        #endregion
    }
}