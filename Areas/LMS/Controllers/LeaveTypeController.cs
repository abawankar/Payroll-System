using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Areas.LMS.Models;

namespace WebPayroll.Areas.LMS.Controllers
{
    [Authorize]
    public class LeaveTypeController : Controller
    {
        // GET: LeaveType
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                LeaveTypeRepository dal = new LeaveTypeRepository();
                List<LeaveTypeModel> model = new List<LeaveTypeModel>();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<LeaveTypeModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(LeaveTypeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveTypeRepository dal = new LeaveTypeRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(LeaveTypeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveTypeRepository dal = new LeaveTypeRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetLeaveTypeOptions()
        {
            List<LeaveTypeModel> model = new List<LeaveTypeModel>();
            model.Add(new LeaveTypeModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                LeaveTypeRepository dal = new LeaveTypeRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}