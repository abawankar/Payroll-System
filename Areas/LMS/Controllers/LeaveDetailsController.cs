using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPayroll.Areas.LMS.Models;
using WebPayroll.DAL.LMS;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.LMS;
using WebPayroll.Domain.Interfaces.LMS;
using WebPayroll.Models.Master;

namespace WebPayroll.Areas.LMS.Controllers
{
    [Authorize]
    public class LeaveDetailsController : Controller
    {
        static List<LeaveDetailsHeader> missingempdata = new List<LeaveDetailsHeader>();
        static List<LeaveDetailsHeader> exportList = new List<LeaveDetailsHeader>();

        // GET: LMS/LeaveDetails
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 5)
                .Select(i => new SelectListItem { Value = now.AddYears(-i).ToString("yyyy"), Text = now.AddYears(-i).ToString("yyyy") });
            ViewBag.year = new SelectList(data, "Value", "Text");

            

            return View();
        }

        [HttpPost]
        public JsonResult List(int branch = 0,int yr=0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                LeaveDetailsRepository dal = new LeaveDetailsRepository();
                List<LeaveDetailsHeader> model = new List<LeaveDetailsHeader>();
                var data = dal.GetActiveByBranch(branch,yr).ToList();
                foreach (var year in data)
                {
                    LeaveDetailsHeader bl = new LeaveDetailsHeader();
                    bl.Id = year.Id;
                    bl.Year = year.Year;
                    bl.BranchId = year.Employee.Branch.Id;
                    bl.BranchName = year.Employee.Branch.Name;
                    bl.EmpId = year.Employee.Id;
                    bl.EmpName = year.Employee.Name;
                    bl.DOJ = year.Employee.DOJ;
                    bl.PaidLeave = year.PaidLeave;
                    bl.Extra = year.Extra;
                    bl.Unpaid = year.UnPaid;
                    bl.Total = year.JAN + year.FEB + year.MAR + year.APR + year.MAY + year.JUN + year.JUL + year.AUG + year.SEP + year.OCT + year.NOV + year.DEC;
                    bl.Deducted = year.Deduction.Sum(x => x.JAN + x.FEB + x.MAR + x.APR + x.MAY + x.JUN + x.JUL + x.AUG + x.SEP + x.OCT + x.NOV + x.DEC);
                    model.Add(bl);
                }
                if(!string.IsNullOrEmpty(search))
                {
                    model = model.Where(x => (x.EmpName + "").ToLower().Contains(search.ToLower())).ToList();
                }
                exportList = model.ToList();
                int count = model.Count;
                List<LeaveDetailsHeader> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult lvList(int id)
        {
            try
            {
                LeaveDetailsRepository dal = new LeaveDetailsRepository();
                List<LeaveDetailsModel> model = new List<LeaveDetailsModel>();
                model.Add(dal.GetById(id));
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
            
        }

        [HttpPost]
        public JsonResult lvupdate(LeaveDetailsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveDetailsRepository dal = new LeaveDetailsRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult lvdList(int id)
        {
            try
            {
                LeaveDeductionRepository dal = new LeaveDeductionRepository();
                List<LeaveDeductionModel> model = new List<LeaveDeductionModel>();
                model = dal.GetAll(id).ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult lvdupdate(LeaveDeductionModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                LeaveDeductionRepository dal = new LeaveDeductionRepository();
                dal.Edit(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult Export()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
            dt.Columns.Add(new DataColumn("Year", typeof(string)));
            dt.Columns.Add(new DataColumn("Branch", typeof(string)));
            dt.Columns.Add(new DataColumn("Employee", typeof(string)));
            dt.Columns.Add(new DataColumn("DOJ", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalExp", typeof(string)));
            dt.Columns.Add(new DataColumn("PaidLeave", typeof(string)));
            dt.Columns.Add(new DataColumn("Extra", typeof(string)));
            dt.Columns.Add(new DataColumn("Unpaid", typeof(string)));
            dt.Columns.Add(new DataColumn("Total", typeof(string)));
            dt.Columns.Add(new DataColumn("Balance", typeof(string)));
            dt.Columns.Add(new DataColumn("Deducted", typeof(string)));
            dt.Columns.Add(new DataColumn("Pending", typeof(string)));

            GridView gv = new GridView();
            var data = exportList.ToList();
            int i = 1;
            string branch = null;
            foreach (var item in data)
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["Year"] = item.Year;
                dr["Branch"] = branch= item.BranchName;
                dr["Employee"] = item.EmpName;
                dr["DOJ"] = Convert.ToDateTime(item.DOJ).ToString("dd/MMM/yyyy");
                dr["TotalExp"] = item.TotalExp;
                dr["PaidLeave"] = item.PaidLeave;
                dr["Extra"] = item.Extra;
                dr["Unpaid"] = item.Unpaid;
                dr["Total"] = item.Total;
                dr["Balance"] = item.Balance;
                dr["Deducted"] = item.Deducted;
                if(Convert.ToDouble(item.Balance)<0)
                {
                    dr["Pending"] = -Convert.ToDouble(item.Balance) - (Convert.ToDouble(item.Deducted)- Convert.ToDouble(item.Unpaid));
                }
                
                dt.Rows.Add(dr);
                i++;
            }
           
            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "LeaveDetails-" + branch + ".xls";
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

        public ActionResult TimingList(int EmpCode, int month, int year)
        {
            MonthlyAttendanceRepository dal = new MonthlyAttendanceRepository();
            List<MonthlyAttendanceModel> model = dal.GetByEmp(EmpCode).Where(x=>x.Month==month && x.Year == year).ToList();

            var dates = new List<DateTime>();

            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            ViewBag.cal = dates;

            return PartialView(model.OrderBy(x => x.Days));
        }

        public ActionResult ExportAttendance(int branch,int month,int year)
        {
            string brn = null;
            var dates = new List<DateTime>();
            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Sr.No", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpName", typeof(string)));
            foreach (var item in dates)
            {
                dt.Columns.Add(new DataColumn(item.Day.ToString() + " " + item.ToString("ddd"), typeof(string)));
            }

            LeaveDetailsRepository dal = new LeaveDetailsRepository();
            MonthlyAttendanceRepository a = new MonthlyAttendanceRepository();
            List<LeaveDetailsModel> bl = dal.GetAll().Where(x => x.Year == year && x.Employee.Branch.Id == branch).ToList();
            DateTime ftd = new DateTime(year, month, 1);
            bl = bl.Where(x => x.Employee.DOJ <= ftd && x.Employee.Status ==1).ToList();
            GridView gv = new GridView();
            int i = 1;
            foreach (var item in bl)
            {
                DataRow dr = dt.NewRow();
                dr["Sr.No"] = i;
                dr["EmpName"] = item.Employee.Name;
                brn = item.Employee.Branch.Name;

                var d = a.GetAll().Where(x => x.Employee.Id == item.Employee.Id && x.Month == month && x.Year == year).ToList();

                foreach (var c in dates)
                {
                    var l = d.Where(x => x.Days == c.Day).SingleOrDefault();
                    if(l!= null)
                    {
                        dr[c.Day.ToString() + " " + c.ToString("ddd")] = l.LeaveType;
                    }
                    else
                    {
                        if(c>=System.DateTime.Today)
                        {
                            dr[c.Day.ToString() + " " + c.ToString("ddd")] = "";
                        }
                        else
                        {
                            dr[c.Day.ToString() + " " + c.ToString("ddd")] = "P";
                        }
                    }
                }
                i++;
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "MonthlyAttendance-" + brn + "-" + month + ".xls";
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

        #region ---- Add missing employee ----

        public ActionResult MissingEmployee()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 1)
                .Select(i => new SelectListItem { Value = now.AddYears(-i).ToString("yyyy"), Text = now.AddYears(-i).ToString("yyyy") });
            ViewBag.year = new SelectList(data, "Value", "Text");

            return View();

        }

        [HttpPost]
        public JsonResult MissingEmpList(int branch)
        {
            missingempdata.Clear();
            try
            {
                LeaveDetailsDAL dal = new LeaveDetailsDAL();
                var empid = from m in dal.GetActiveByBranch(branch, System.DateTime.Today.Year).ToList()
                            select m.Employee.Id;

                EmployeeDAL empdal = new EmployeeDAL();
                var emp = empdal.GetActiveByBranch(branch).Where(x => !(empid.Contains(x.Id))).ToList();

                
                foreach (var item in emp.OrderBy(x=>x.Name))
                {
                    LeaveDetailsHeader bl = new LeaveDetailsHeader();
                    bl.Year = System.DateTime.Today.Year;
                    bl.EmpId = item.Id;
                    bl.BranchId = item.Branch.Id;
                    bl.EmpName = item.Name;
                    bl.DOJ = item.DOJ;
                    bl.PaidLeave = item.Designation.PaidLeave;
                    bl.Extra = 0;
                    missingempdata.Add(bl);
                }
                var model = missingempdata.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult updateMissingEmp(LeaveDetailsHeader model)
        {
            try
            {
                var data = missingempdata.Where(x => x.Id == model.Id).SingleOrDefault();
                if (model.PaidLeave != 0) data.PaidLeave = model.PaidLeave;
                if (model.Extra != 0) data.Extra = model.Extra;
                
                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Process()
        {
            string message = "";

            List<ILeaveDetails> list = new List<ILeaveDetails>();
            foreach (var obj in missingempdata)
            {
               ILeaveDetails bl = new LeaveDetails();
                bl.Year = obj.Year;
                bl.Employee = new Employee { Id = obj.EmpId };
                bl.PaidLeave = obj.PaidLeave;
                bl.Extra = obj.Extra;

                ILeaveDeduction de = new LeaveDeduction();
                de.Year = obj.Year;
                de.FEB = 0;
                de.MAR = 0;
                de.APR = 0;
                de.MAY = 0;
                de.JUN = 0;
                de.JUL = 0;
                de.AUG = 0;
                de.SEP = 0;
                de.OCT = 0;
                de.NOV = 0;
                de.DEC = 0;
                bl.Deduction.Add(de);
                list.Add(bl);
            }
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            dal.InsertBulk(list);
            message = "Saved Successfully";
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public JsonResult GetEmpLeaveDetails(int id)
        {
            try
            {
                LeaveDetailsRepository dal = new LeaveDetailsRepository();
                List<LeaveDetailsModel> model = new List<LeaveDetailsModel>();
                model = dal.GetAll().Where(x => x.Employee.Id == id).ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }

        }
    }
}