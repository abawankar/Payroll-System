using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Master
{
    [Authorize]//103
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("M003"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                DepartmentRepository dal = new DepartmentRepository();
                List<DepartmentModel> model = new List<DepartmentModel>();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<DepartmentModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(DepartmentModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    DepartmentRepository dal = new DepartmentRepository();
                    dal.Insert(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(DepartmentModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    DepartmentRepository dal = new DepartmentRepository();
                    dal.Edit(model);
                    return Json(new { Result = "OK", Record = model });
                }
                else
                {
                    return Json(new { Result = "Error", Message = "Sorry, You are not Authorized to do this action" });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetDepartmentOptions()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            model.Add(new DepartmentModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                DepartmentRepository dal = new DepartmentRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list.OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}