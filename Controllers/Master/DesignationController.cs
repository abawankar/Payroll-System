using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Master
{
    [Authorize]//104
    public class DesignationController : Controller
    {
        // GET: Designation
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("M004"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                DesignationRepository dal = new DesignationRepository();
                List<DesignationModel> model = new List<DesignationModel>();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<DesignationModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(DesignationModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    DesignationRepository dal = new DesignationRepository();
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
        public JsonResult Update(DesignationModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    DesignationRepository dal = new DesignationRepository();
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
        public JsonResult GetDesignationOptions()
        {
            List<DesignationModel> model = new List<DesignationModel>();
            model.Add(new DesignationModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                DesignationRepository dal = new DesignationRepository();
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