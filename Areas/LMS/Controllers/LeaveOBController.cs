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
    public class LeaveOBController : Controller
    {
        // GET: LeaveOB
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            return View();
        }

        [HttpPost]
        public JsonResult List(int branch = 0,string search=null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                LeaveOBRepository dal = new LeaveOBRepository();
                List<LeaveOBModel> model = new List<LeaveOBModel>();
                model = dal.GetAll().ToList();
                if(branch != 0)
                {
                    model = model.Where(x => x.Employee.Branch.Id == branch).ToList();
                }
                if(!string.IsNullOrEmpty(search))
                {
                    model = model.Where(x => (x.Employee.Name + " " + x.Year.ToString()).ToLower().Contains(search.ToLower())).ToList();
                }
                int count = model.Count;
                List<LeaveOBModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(LeaveOBModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveOBRepository dal = new LeaveOBRepository();
                if(dal.IsLeaveAdded(model.EmpId,model.LeaveTypeId,model.Year).Count()==0)
                {
                    dal.Insert(model);
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "Leave type alredy added for this year" });
                }
                
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(LeaveOBModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveOBRepository dal = new LeaveOBRepository();
                dal.Edit(model);
                model = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


    }
}