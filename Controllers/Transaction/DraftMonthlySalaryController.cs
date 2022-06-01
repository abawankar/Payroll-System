using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPayroll.Common;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;
using WebPayroll.Models.Transaction;

namespace WebPayroll.Controllers.Transaction
{
    [Authorize]
    public class DraftMonthlySalaryController : Controller
    {

        #region ---- DraftMonthlySalary ---

        static List<DraftMonthlySalaryModel> draftSalary = new List<DraftMonthlySalaryModel>();
        static List<SalaryFooterModel> salaryfooter = new List<SalaryFooterModel>();
        // GET: DraftMonthlySalary
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 2)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.Months = new SelectList(data, "Value", "Text");

            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("T003"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int branch = 0,string month=null,int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            draftSalary.Clear();
            try
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                LoanDRTranRepository loandl = new LoanDRTranRepository();
                var data = dal.GetAll(1).Where(x => x.Employee.Branch.Id == branch).ToList();
                List<DraftMonthlySalaryModel> list = new List<DraftMonthlySalaryModel>();
                foreach (var item in data)
                {
                    DraftMonthlySalaryModel bl = new DraftMonthlySalaryModel();
                    bl.Id = item.Id;
                    bl.BranId = branch;
                    bl.EmpId = item.Employee.Id;
                    bl.Basic = item.Basic;
                    bl.DA = item.DA;
                    bl.HRA = item.HRA;
                    bl.Conveyance = item.Conveyance;
                    bl.Medical = item.Medical;
                    bl.EduAllowance = item.EduAllowance;
                    bl.TelephoneReimb = item.TelephoneReimb;
                    bl.CarRunningReimb = item.CarRunningReimb;
                    bl.SatutoryBonus = item.SatutoryBonus;
                    bl.OtherAllowance = item.OtherAllowance;
                    bl.PF = item.PF;
                    bl.VPF = item.VPF;
                    bl.ESI = item.ESI;
                    bl.TDS = item.TDS;
                    bl.Loan = loandl.GetMonthDRId(bl.EmpId, month);
                    bl.LoanId = bl.Loan.Id;
                    bl.PaidDays = month.NoOfDaysInMonth();
                    bl.MontYear = month;
                    list.Add(bl);
                }
                draftSalary = list;
                int count = draftSalary.Count;
                return Json(new { Result = "OK", Records = draftSalary, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(DraftMonthlySalaryModel model)
        {
            try
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                SalaryMasterModel bl = dal.GetById(model.Id);
                
                var data = draftSalary.Where(x => x.Id == model.Id).SingleOrDefault();
                int days = data.MontYear.NoOfDaysInMonth();
                data.Basic = Math.Round((bl.Basic / days) * model.PaidDays);
                data.DA = Math.Round((bl.DA / days) * model.PaidDays);
                data.HRA = Math.Round((bl.HRA / days) * model.PaidDays);
                data.Conveyance = Math.Round((bl.Conveyance / days) * model.PaidDays);
                data.Medical = Math.Round((bl.Medical / days) * model.PaidDays);
                data.EduAllowance = Math.Round((bl.EduAllowance / days) * model.PaidDays);
                data.TelephoneReimb = Math.Round((bl.TelephoneReimb / days) * model.PaidDays);
                data.CarRunningReimb = Math.Round((bl.CarRunningReimb / days) * model.PaidDays);
                data.SatutoryBonus = Math.Round((bl.SatutoryBonus / days) * model.PaidDays);
                data.OtherAllowance = Math.Round((bl.OtherAllowance / days) * model.PaidDays);
                data.PaidDays = model.PaidDays;

                if(bl.IsPFExempted =="N")
                {
                    if(bl.IsPFCelling == "Y")
                    {
                        data.PF = data.Basic > bl.Employee.StatutoryCode.PFCelling
                            ? 1800 : Math.Round((data.Basic * 12)/100);
                    }
                    else
                    {
                        data.PF = Math.Round((data.Basic * 12)/100);
                    }
                }
                else
                {
                    data.PF = 0;
                }

                if (bl.IsESIExempted == "N")
                {
                    data.ESI = Math.Ceiling((data.GrossSalary * bl.Employee.StatutoryCode.ESICont)/100);
                }
                else
                {
                    data.ESI = 0;
                }
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SalaryFooter()
        {
            salaryfooter.Clear();
            List<SalaryFooterModel> list = new List<SalaryFooterModel>();
            try
            {
                SalaryFooterModel bl = new SalaryFooterModel();
                bl.NoOfEmp = draftSalary.Count();
                bl.Basic = draftSalary.Sum(x=>x.Basic);
                bl.DA = draftSalary.Sum(x => x.DA);
                bl.HRA = draftSalary.Sum(x => x.HRA);
                bl.Conveyance = draftSalary.Sum(x => x.Conveyance);
                bl.Medical = draftSalary.Sum(x => x.Medical);
                bl.EduAllowance = draftSalary.Sum(x => x.EduAllowance);
                bl.TelephoneReimb = draftSalary.Sum(x => x.TelephoneReimb);
                bl.CarRunningReimb = draftSalary.Sum(x => x.CarRunningReimb);
                bl.SatutoryBonus = draftSalary.Sum(x => x.SatutoryBonus);
                bl.OtherAllowance = draftSalary.Sum(x => x.OtherAllowance);
                bl.PF = draftSalary.Sum(x => x.PF);
                bl.VPF = draftSalary.Sum(x => x.VPF);
                bl.ESI = draftSalary.Sum(x => x.ESI);
                bl.TDS = draftSalary.Sum(x => x.TDS);
                bl.LoanAmount = draftSalary.Sum(x => x.LoanAmount);
                list.Add(bl);
                salaryfooter = list;
                int count = salaryfooter.Count;
                return Json(new { Result = "OK", Records = salaryfooter, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveDraft(string month)
        {
            string message = "";

            DraftMonthlySalaryDAL dal = new DraftMonthlySalaryDAL();
            List<IDraftMonthlySalary> draft = new List<IDraftMonthlySalary>();
            foreach (var item in draftSalary)
            {
                IDraftMonthlySalary bl = new DraftMonthlySalary();
                bl.Branch = new Branch { Id = item.BranId };
                bl.Employee = new Employee { Id = item.EmpId};
                bl.Basic = item.Basic;
                bl.DA = item.DA;
                bl.HRA = item.HRA;
                bl.Conveyance = item.Conveyance;
                bl.Medical = item.Medical;
                bl.EduAllowance = item.EduAllowance;
                bl.TelephoneReimb = item.TelephoneReimb;
                bl.CarRunningReimb = item.CarRunningReimb;
                bl.SatutoryBonus = item.SatutoryBonus;
                bl.OtherAllowance = item.OtherAllowance;
                bl.PF = item.PF;
                bl.VPF = item.VPF;
                bl.ESI = item.ESI;
                bl.TDS = item.TDS;
                if(item.LoanId != 0)
                bl.Loan = new LoanDRTran { Id = item.LoanId };
                bl.PaidDays = item.PaidDays;
                bl.MontYear = item.MontYear;
                draft.Add(bl);
            }
            dal.InsertBulk(draft);
            message = "Process Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ------ View Draft Salary -------
        public ActionResult ViewDraftSalary()
        {
            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("T004"))
            { return View(); }
            else { return View("_Security"); }
        }

        static List<DraftMonthlySalaryModel> viewDraftSalary = new List<DraftMonthlySalaryModel>();
        static List<SalaryFooterModel> draftSalaryFooter = new List<SalaryFooterModel>();

        [HttpPost]
        public JsonResult ViewDraftList(int branch = 0, string month = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                DraftMonthlySalaryRepository dal = new DraftMonthlySalaryRepository();
                List<SalaryFooterModel> list = new List<SalaryFooterModel>();

                foreach (var br in dal.GetAll().GroupBy(x=>x.Branch))
                {
                    foreach (var item in br.GroupBy(x=>x.MontYear))
                    {
                        SalaryFooterModel bl = new SalaryFooterModel();
                        bl.BranchId = br.Key.Id;
                        bl.BranCode = br.Key.Name;
                        bl.MonthYear = item.Key;
                        bl.NoOfEmp = item.Count();
                        bl.Basic = item.Sum(x => x.Basic);
                        bl.DA = item.Sum(x => x.DA);
                        bl.HRA = item.Sum(x => x.HRA);
                        bl.Conveyance = item.Sum(x => x.Conveyance);
                        bl.Medical = item.Sum(x => x.Medical);
                        bl.EduAllowance = item.Sum(x => x.EduAllowance);
                        bl.TelephoneReimb = item.Sum(x => x.TelephoneReimb);
                        bl.CarRunningReimb = item.Sum(x => x.CarRunningReimb);
                        bl.SatutoryBonus = item.Sum(x => x.SatutoryBonus);
                        bl.OtherAllowance = item.Sum(x => x.OtherAllowance);
                        bl.PF = item.Sum(x => x.PF);
                        bl.VPF = item.Sum(x => x.VPF);
                        bl.ESI = item.Sum(x => x.ESI);
                        bl.TDS = item.Sum(x => x.TDS);
                        bl.LoanAmount = item.Sum(x => x.LoanAmount);
                        list.Add(bl);
                    }
                    
                }
                int count = list.Count;
                return Json(new { Result = "OK", Records = list, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DraftFullDetails(string brancode=null,string monthyear=null)
        {
            try
            {
                DraftMonthlySalaryRepository dal = new DraftMonthlySalaryRepository();
                List<DraftMonthlySalaryModel> model = new List<DraftMonthlySalaryModel>();
                model = dal.GetAll().Where(x => x.Branch.Name == brancode && x.MontYear == monthyear).ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateDraft(DraftMonthlySalaryModel model)
        {
            try
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                

                DraftMonthlySalaryRepository draftdal = new DraftMonthlySalaryRepository();
                DraftMonthlySalaryModel data = draftdal.GetById(model.Id);
                SalaryMasterModel bl = dal.GetAll().Where(x => x.Employee.Id == data.Employee.Id).SingleOrDefault();

                int days = data.MontYear.NoOfDaysInMonth();
                data.Basic = Math.Round((bl.Basic / days) * model.PaidDays);
                data.DA = Math.Round((bl.DA / days) * model.PaidDays);
                data.HRA = Math.Round((bl.HRA / days) * model.PaidDays);
                data.Conveyance = Math.Round((bl.Conveyance / days) * model.PaidDays);
                data.Medical = Math.Round((bl.Medical / days) * model.PaidDays);
                data.EduAllowance = Math.Round((bl.EduAllowance / days) * model.PaidDays);
                data.TelephoneReimb = Math.Round((bl.TelephoneReimb / days) * model.PaidDays);
                data.CarRunningReimb = Math.Round((bl.CarRunningReimb / days) * model.PaidDays);
                data.SatutoryBonus = Math.Round((bl.SatutoryBonus / days) * model.PaidDays);
                data.OtherAllowance = Math.Round((bl.OtherAllowance / days) * model.PaidDays);
                data.PaidDays = model.PaidDays;
                data.TDS = model.TDS;
                if (bl.IsPFExempted == "N")
                {
                    if (bl.IsPFCelling == "Y")
                    {
                        data.PF = data.Basic > bl.Employee.StatutoryCode.PFCelling
                            ? 1800 : Math.Round((data.Basic * bl.Employee.StatutoryCode.PFCont) / 100);
                    }
                    else
                    {
                        data.PF = Math.Round((data.Basic * bl.Employee.StatutoryCode.PFCont) / 100);
                    }
                }
                else
                {
                    data.PF = 0;
                }

                if (bl.IsESIExempted == "N")
                {
                    data.ESI = Math.Ceiling((data.GrossSalary * bl.Employee.StatutoryCode.ESICont) / 100);
                }
                else
                {
                    data.ESI = 0;
                }
                draftdal.Edit(data);
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult ECRxls(int branch, string month)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("UAN", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Basic", typeof(string)));
            dt.Columns.Add(new DataColumn("HRA", typeof(string)));

            dt.Columns.Add(new DataColumn("EduAllowance", typeof(string)));
            dt.Columns.Add(new DataColumn("TelephoneReimb", typeof(string)));
            dt.Columns.Add(new DataColumn("SatutoryBonus", typeof(string)));
            dt.Columns.Add(new DataColumn("CarRunningReimb", typeof(string)));
            dt.Columns.Add(new DataColumn("OtherAllowance", typeof(string)));

            dt.Columns.Add(new DataColumn("Conveyance", typeof(string)));
            dt.Columns.Add(new DataColumn("Medical", typeof(string)));
            dt.Columns.Add(new DataColumn("GrossSalary", typeof(string)));
            dt.Columns.Add(new DataColumn("PF", typeof(string)));
            dt.Columns.Add(new DataColumn("VPF", typeof(string)));
            dt.Columns.Add(new DataColumn("ESI", typeof(string)));
            dt.Columns.Add(new DataColumn("TDS", typeof(string)));
            dt.Columns.Add(new DataColumn("LoanAmount", typeof(string)));
            dt.Columns.Add(new DataColumn("NetDedn", typeof(string)));
            dt.Columns.Add(new DataColumn("NetSalary", typeof(string)));
            dt.Columns.Add(new DataColumn("PaidDays", typeof(string)));
            dt.Columns.Add(new DataColumn("NCPDays", typeof(string)));

            GridView gv = new GridView();
            DraftMonthlySalaryRepository dal = new DraftMonthlySalaryRepository();
            var data = dal.GetAll().Where(x => x.Branch.Id == branch && x.MontYear == month).ToList().OrderBy(x=>x.Employee.Name);
            int i = 1;
            int days = month.NoOfDaysInMonth();
            int ncpdays = 0;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["UAN"] = item.Employee.UAN;
                dr["EmpCode"] = item.Employee.EmpCode;
                dr["Name"] = item.Employee.Name;
                dr["Basic"] = item.Basic;
                dr["HRA"] = item.HRA;

                dr["EduAllowance"] = item.EduAllowance;
                dr["TelephoneReimb"] = item.TelephoneReimb;
                dr["SatutoryBonus"] = item.SatutoryBonus;
                dr["CarRunningReimb"] = item.CarRunningReimb;
                dr["OtherAllowance"] = item.OtherAllowance;


                dr["Conveyance"] = item.Conveyance;
                dr["Medical"] = item.Medical;
                dr["GrossSalary"] = item.GrossSalary;
                dr["PF"] = item.PF;
                dr["VPF"] = item.VPF;
                dr["ESI"] = item.ESI;
                dr["TDS"] = item.TDS;
                dr["LoanAmount"] = item.LoanAmount;
                dr["NetDedn"] = item.NetDedn;
                dr["NetSalary"] = item.NetSalary;
                dr["PaidDays"] = item.PaidDays;
                ncpdays += days - item.PaidDays;
                dr["NCPDays"] = days - item.PaidDays;
                dt.Rows.Add(dr);
                i++;
            }
            DataRow dr1 = dt.NewRow();
            dr1["UAN"] = "Total";
            dr1["Basic"] = data.Sum(x => x.Basic);
            dr1["HRA"] = data.Sum(x => x.HRA);

            dr1["EduAllowance"] = data.Sum(x => x.EduAllowance);
            dr1["TelephoneReimb"] = data.Sum(x => x.TelephoneReimb);
            dr1["SatutoryBonus"] = data.Sum(x => x.SatutoryBonus);
            dr1["CarRunningReimb"] = data.Sum(x => x.CarRunningReimb);
            dr1["OtherAllowance"] = data.Sum(x => x.OtherAllowance);

            dr1["Conveyance"] = data.Sum(x => x.Conveyance);
            dr1["Medical"] = data.Sum(x => x.Medical);
            dr1["GrossSalary"] = data.Sum(x => x.GrossSalary);
            dr1["PF"] = data.Sum(x => x.PF);
            dr1["VPF"] = data.Sum(x => x.VPF);
            dr1["ESI"] = data.Sum(x => x.ESI);
            dr1["TDS"] = data.Sum(x => x.TDS);
            dr1["LoanAmount"] = data.Sum(x => x.LoanAmount);
            dr1["NetDedn"] = data.Sum(x => x.NetDedn);
            dr1["NetSalary"] = data.Sum(x => x.NetSalary);
            dr1["NCPDays"] = ncpdays;
            dt.Rows.Add(dr1);

            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName =  "Salary-" + month + ".xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("ViewDraftSalary");
        }
        #endregion

        #region --- Process Salary -----

        [HttpPost]
        public JsonResult ProcessSalary(int branch = 0, string month = null)
        {
            if (User.IsInRole("Admin")) { }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("T005"))
            {
            }
            else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

            MonthlySalaryMasterDAL dal = new MonthlySalaryMasterDAL();
            if (dal.GetAll().Where(x => x.Branch.Id == branch && x.MonthYear == month).Count() > 0)
            {
                return Json("Salary Alredy Porcess for -" + month, JsonRequestBehavior.AllowGet);
            }

            string message = "";

            DraftMonthlySalaryRepository draft = new DraftMonthlySalaryRepository();
            var data = draft.GetAll().Where(x => x.Branch.Id == branch && x.MontYear == month).ToList();

            IMonthlySalaryMaster salary = new MonthlySalaryMaster();
            salary.Branch = new Branch { Id = branch };
            salary.MonthYear = month;
            salary.Cheque = "00000";
            salary.Date = Convert.ToDateTime(System.DateTime.Today);

            foreach (var sal in data)
            {
                IMonthlySalary bl = new MonthlySalary();
                bl.MonthYear = month;
                bl.Basic = sal.Basic;
                bl.DA = sal.DA;
                bl.HRA = sal.HRA;
                bl.Medical = sal.Medical;
                bl.Conveyance = sal.Conveyance;
                bl.EduAllowance = sal.EduAllowance;
                bl.TelephoneReimb = sal.TelephoneReimb;
                bl.CarRunningReimb = sal.CarRunningReimb;
                bl.SatutoryBonus = sal.SatutoryBonus;
                bl.OtherAllowance = sal.OtherAllowance;
                bl.PF = sal.PF;
                bl.VPF = sal.VPF;
                bl.ESI = sal.ESI;
                bl.TDS = sal.TDS;
                bl.PaidDays = sal.PaidDays;
                bl.Employee = new Employee { Id = sal.Employee.Id };
                bl.LoanAmount = sal.LoanAmount;
                salary.MonthlySalary.Add(bl);

                bl.SalaryArrear = MonthlySalaryDAL.GetSalaryArrear(sal.Employee.Id);
            }

            try
            {
                dal.InsertOrUpdate(salary);
                dal.DeleteDraftReturn(branch, month);
                message = "Process Successfully";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = "Error :";
                return Json(message + " " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteDraft(int branch = 0, string month = null)
        {
            if (User.IsInRole("Admin")) { }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("T006"))
            {
            }
            else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

            MonthlySalaryMasterDAL dal = new MonthlySalaryMasterDAL();
            var data = dal.DeleteDraftReturn(branch,month);
            string message = "Drafted Salary Deleted Successfully!";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}