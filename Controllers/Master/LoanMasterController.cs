using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Master
{
    [Authorize]
    public class LoanMasterController : Controller
    {
        // GET: EmployeeLoan
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "Name", Value = "1" },
                new SelectListItem { Text = "Loan Code", Value = "2" },
            }, "Value", "Text");


            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M017")) { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int status = 0, int branch = 0, int field = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                LoanMasterRepository dal = new LoanMasterRepository();
                List<LoanMasterModel> model = new List<LoanMasterModel>();
                model = dal.GetAll().ToList();
                if (branch != 0)
                {
                    model = model.Where(x => x.Employee.Branch.Id == branch).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => (x.Employee.Name + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                        case 2:
                            model = model.Where(x => x.LoanCode == search.Trim()).ToList();
                            break;
                    }
                }
                int count = model.Count;
                model = model.OrderBy(x => x.Date).ToList();
                List<LoanMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(LoanMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LoanMasterRepository dal = new LoanMasterRepository();
                
                if (User.IsInRole("Admin")) { dal.Insert(model); }
                else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M018")) { dal.Insert(model); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

                return Json(new { Result = "OK", Record = model });
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
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LoanMasterRepository dal = new LoanMasterRepository();
                if (User.IsInRole("Admin"))
                {
                    dal.Edit(model);
                    model = dal.GetById(model.Id);
                }
                else if (Models.Admin.UserRightRepository
                    .RightList(User.Identity.Name).Contains("M018"))
                {
                    dal.Edit(model);
                    model = dal.GetById(model.Id);
                }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #region  ---- Loan CR Tran ---

        [HttpPost]
        public JsonResult LoanCRList(int loanid)
        {
            try
            {
                LoanCRTranRepository dal = new LoanCRTranRepository();
                List<LoanCRTranModel> model = new List<LoanCRTranModel>();
                model = dal.GetAll(loanid).ToList();
                int count = model.Count;
                model = model.OrderBy(x => x.Date).ToList();
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult LoanCRUpdate(LoanCRTranModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LoanCRTranRepository dal = new LoanCRTranRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult LoanCRCreate(LoanCRTranModel model,int loanid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LoanCRTranRepository dal = new LoanCRTranRepository();
                dal.Insert(model,loanid);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #endregion

        #region  ---- Loan DR Tran ---

        [HttpPost]
        public JsonResult LoanDRList(int loanid)
        {
            try
            {
                LoanDRTranRepository dal = new LoanDRTranRepository();
                List<LoanDRTranModel> model = new List<LoanDRTranModel>();
                model = dal.GetAll(loanid).ToList();
                int count = model.Count;
                model = model.OrderByDescending(x => x.Date).ToList();
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult LoanDRUpdate(LoanDRTranModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LoanDRTranRepository dal = new LoanDRTranRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult LoanDRCreate(LoanDRTranModel model, int loanid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                if(model.PaidBy == "S")
                {
                    return Json(new { Result = "ERROR", Message = "Cannot process salary deduction from here." });
                }
                LoanDRTranRepository dal = new LoanDRTranRepository();
                dal.Insert(model, loanid);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #endregion

        #region --- Report ---

        public ActionResult LoanOutstandingReport()
        {
            LoanMasterRepository dal = new LoanMasterRepository();
            var data = dal.GetAll().Where(x => x.BalanceAmount != 0).ToList();
            ReportDataSet rds = new ReportDataSet();
            int i = 1;
            foreach (var item in data.OrderBy(x=>x.Employee.Name))
            {
                DataRow dr = rds.Tables["LoanMaster"].NewRow();
                dr["Id"] = i;
                dr["EmpName"] = item.Employee.Name;
                dr["LoanAmount"] = item.LoanCRTran.Sum(x => x.Amount);
                dr["PaidAmount"] = item.LoanDRTran.Sum(x => x.Amount);
                dr["BalanceAmount"] = item.BalanceAmount;
                rds.Tables["LoanMaster"].Rows.Add(dr);
                i++;
            }
            
            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\LoanOutstanding.rpt";
            var reportpath = Server.MapPath(repoertname);
            rd.Load(reportpath);
            rd.SetDataSource(rds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/pdf", "LoanOutstandingReport.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}