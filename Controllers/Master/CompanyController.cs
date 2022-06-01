using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Master
{
    [Authorize]//101
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") ||AppsUserRepository.RightList(User.Identity.Name).Contains("M001"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                CompanyRepository dal = new CompanyRepository();
                List<CompanyModel> model = new List<CompanyModel>();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<CompanyModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(CompanyModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    CompanyRepository dal = new CompanyRepository();
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
        public JsonResult Update(CompanyModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    CompanyRepository dal = new CompanyRepository();
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
        public JsonResult GetCompanyOptions()
        {
            List<CompanyModel> model = new List<CompanyModel>();
            //model.Add(new CompanyModel { Id = 0, Code = "* Select *" });
            //var data = model.Select(c => new { DisplayText = c.Code, Value = c.Id });
            try
            {
                CompanyRepository dal = new CompanyRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Code, Value = c.Id });
                return Json(new { Result = "OK", Options = list.OrderBy(x => x.DisplayText) });
                //return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}