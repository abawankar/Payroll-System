using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Transaction
{
    [Authorize]
    public class MonthlyDeductionController : Controller
    {
        static List<LoanMasterModel> draftlist = new List<LoanMasterModel>();
        // GET: MonthlyDeduction
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 2)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.Months = new SelectList(data, "Value", "Text");

            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("T002"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int branch = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                LoanMasterRepository dal = new LoanMasterRepository();
                List<LoanMasterModel> model = new List<LoanMasterModel>();
                if(branch != 0)
                {
                    model = dal.GetAll().Where(x => x.Employee.Branch.Id == branch && x.BalanceAmount != 0).ToList();
                }
                int count = model.Count;
                draftlist = model.ToList();
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(LoanMasterModel model)
        {
            try
            {

                var data = draftlist.Where(x => x.Id == model.Id).SingleOrDefault();
                if (model.DednAmount != 0) data.DednAmount = model.DednAmount;
                if (model.PaidBy != null) data.PaidBy = model.PaidBy;

                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Process(string month)
        {
            string message = "";

            LoanMasterDAL dal = new LoanMasterDAL();
            foreach (var obj in draftlist)
            {
                ILoanMaster bl = dal.GetById(obj.Id);
                ILoanDRTran d = new LoanDRTran();
                d.Date = System.DateTime.Today;
                d.Amount = obj.DednAmount;
                d.PaidMonth = month;
                d.PaidBy = obj.PaidBy;
                bl.LoanDRTran.Add(d);
                dal.InsertOrUpdate(bl);

            }
            message = "Process Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}