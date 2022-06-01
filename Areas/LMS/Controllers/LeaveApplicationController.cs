using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Areas.LMS.Models;
using WebPayroll.Models.Master;

namespace WebPayroll.Areas.LMS.Controllers
{
    [Authorize]
    public class LeaveApplicationController : Controller
    {
        // GET: LMS/LeaveApplication

         public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                LeaveApplicationRepository dal = new LeaveApplicationRepository();
                List<LeaveApplicationModel> model = new List<LeaveApplicationModel>();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.DateFrom).ToList();
                List<LeaveApplicationModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        public ActionResult Create()
        {
            BranchRepository dal = new BranchRepository();
            LeaveTypeRepository type = new LeaveTypeRepository();

            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");
            ViewBag.emp = new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.leavetype = new SelectList(type.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.span = new SelectList(new[] {
                new SelectListItem { Text = "Full Day", Value = "1" },
                new SelectListItem { Text = "Half Day", Value = "2" },
            }, "Value", "Text");

            return View();
        }

        [HttpPost]
        public ActionResult Create(LeaveApplicationModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveApplicationRepository dal = new LeaveApplicationRepository();
                dal.Insert(model);

                return RedirectToAction("Index","Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}