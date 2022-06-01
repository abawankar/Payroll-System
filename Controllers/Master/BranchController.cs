using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Master
{
    [Authorize]//102
    public class BranchController : Controller
    {
        // GET: Branch
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("M002"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int status = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                BranchRepository dal = new BranchRepository();
                List<BranchModel> model = new List<BranchModel>();
                model = dal.GetAll().ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.Name).ToList();
                List<BranchModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(BranchModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    BranchRepository dal = new BranchRepository();
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
        public JsonResult Update(BranchModel model)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    BranchRepository dal = new BranchRepository();
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
        public JsonResult GetBranchOptions()
        {
            List<BranchModel> model = new List<BranchModel>();
            model.Add(new BranchModel { Id = 0, Code = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Code, Value = c.Id });
            try
            {
                BranchRepository dal = new BranchRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Code, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetBranchNameOptions()
        {
            List<BranchModel> model = new List<BranchModel>();
            model.Add(new BranchModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                BranchRepository dal = new BranchRepository();
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