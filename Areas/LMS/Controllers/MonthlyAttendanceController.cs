using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Areas.LMS.Models;
using WebPayroll.Models.Master;

namespace WebPayroll.Areas.LMS.Controllers
{
    public class MonthlyAttendanceController : Controller
    {
        // GET: LMS/MonthlyAttendance
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 5)
                .Select(i => new SelectListItem { Value = now.AddYears(-i).ToString("yyyy"), Text = now.AddYears(-i).ToString("yyyy") });
            ViewBag.year = new SelectList(data, "Value", "Text");

            var months = Enumerable.Range(1, 12).Select(i => new { I = i, M = DateTimeFormatInfo.CurrentInfo.GetMonthName(i) });
            ViewBag.month = new SelectList(months, "I", "M");

            return View();
        }

        [HttpPost]
        public JsonResult List(int branch = 0, int yr = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MonthlyAttendanceRepository dal = new MonthlyAttendanceRepository();
                List<MonthlyAttendanceModel> model = new List<MonthlyAttendanceModel>();
                model = dal.GetAll().ToList();
                if (branch != 0)
                {
                    model = model.Where(x=>x.Employee.Branch.Id == branch).ToList();
                }
                if(yr != 0)
                {
                    model = model.Where(x => x.Year == yr).ToList();
                }
                if(!string.IsNullOrEmpty(search))
                {
                    model = model.Where(x => (x.Employee.Name + "").Contains(search)).ToList();
                }
                
                int count = model.Count;
                List<MonthlyAttendanceModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(MonthlyAttendanceModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlyAttendanceRepository dal = new MonthlyAttendanceRepository();
                dal.Insert(model);

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(MonthlyAttendanceModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlyAttendanceRepository dal = new MonthlyAttendanceRepository();
                dal.Edit(model);
                model = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetEmployeeOptions(int id)
        {
            List<EmployeeModel> model = new List<EmployeeModel>();
            model.Add(new EmployeeModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                if (id == 0)
                {
                    var list = dal.GetAll()
                              .Select(c => new { DisplayText = c.Name, Value = c.Id });
                    return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
                }
                else
                {
                    var list = dal.GetAll().Where(x => x.Branch.Id == id).ToList()
                              .Select(c => new { DisplayText = c.Name, Value = c.Id });
                    return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

    }
}