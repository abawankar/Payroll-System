using CrystalDecisions.CrystalReports.Engine;
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
    public class MonthlySalaryMasterController : Controller
    {
        // GET: MonthlySalaryMaster
        public ActionResult Index()
        {
            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("R001"))
            { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int branch = 0, string month = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
                List<MonthlySalaryMasterModel> model = new List<MonthlySalaryMasterModel>();
                model = dal.GetAll().ToList();

                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<MonthlySalaryMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(MonthlySalaryMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
                dal.Edit(model);
                model = dal.GetById(model.Id);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SalaryDetails(int id = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
                List<MonthlySalaryModel> model = new List<MonthlySalaryModel>();
                model = dal.GetAll(id).ToList();

                int count = model.Count;
                model = model.OrderBy(x => x.Employee.Name).ToList();
                List<MonthlySalaryModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        
        public ActionResult MonthlyEmployeeSalary()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            MonthlySalaryMasterRepository sal = new MonthlySalaryMasterRepository();
            var month = from m in sal.GetAll()
                        select new { month = m.MonthYear };
            ViewBag.Months = new SelectList(month.Distinct(), "month", "month");

            return View();
        }

        [HttpPost]
        public JsonResult EmployeeSalaryList(int branch = 0,string month=null,string search=null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MonthlySalaryRepository dal = new MonthlySalaryRepository();
                List<MonthlySalaryModel> model = new List<MonthlySalaryModel>();
                model = dal.GetAll().ToList();
                if(branch != 0)
                {
                    model = model.Where(x => x.BranchId == branch).ToList();
                }
                if (!string.IsNullOrEmpty(search))
                {
                    model = model.Where(x => (x.Employee.Name + " ").ToLower().Contains(search.ToLower())).ToList();
                }

                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<MonthlySalaryModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public ActionResult ECRxls(int id)
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
            dt.Columns.Add(new DataColumn("Arrear", typeof(string)));
            dt.Columns.Add(new DataColumn("NetSalary", typeof(string)));
            dt.Columns.Add(new DataColumn("PaidDays", typeof(string)));
            dt.Columns.Add(new DataColumn("NCPDays", typeof(string)));

            GridView gv = new GridView();
            MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
            var data = dal.GetById(id);
            int i = 1;
            int days = data.MonthYear.NoOfDaysInMonth();
            int ncpdays = 0;
            double gross = 0;
            double netsal = 0;
            double netdend = 0;
            foreach (var item in data.MonthlySalary.OrderBy(x=>x.Employee.Name))
            {
                DataRow dr = dt.NewRow();
                dr["SrNo"] = i;
                dr["UAN"] = "'" + item.Employee.UAN;
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
                gross = item.Basic + item.HRA + + item.EduAllowance + item.TelephoneReimb + item.SatutoryBonus + item.CarRunningReimb + item.OtherAllowance + item.Conveyance + item.Medical;
                dr["GrossSalary"] = item.Basic + item.HRA + +item.EduAllowance + item.TelephoneReimb + item.SatutoryBonus + item.CarRunningReimb + item.OtherAllowance + item.Conveyance + item.Medical;
                dr["PF"] = item.PF;
                dr["VPF"] = item.VPF;
                dr["ESI"] = item.ESI;
                dr["TDS"] = item.TDS;
                dr["LoanAmount"] = item.LoanAmount;
                netdend = item.PF + item.ESI + item.VPF + item.TDS + item.LoanAmount;
                dr["NetDedn"] = item.PF + item.ESI + item.VPF + item.TDS + item.LoanAmount;
                netsal += gross + item.Arrear - netdend;
                dr["Arrear"] = item.Arrear;
                dr["NetSalary"] = gross + item.Arrear- netdend;
                dr["PaidDays"] = item.PaidDays;
                ncpdays += days - item.PaidDays;
                dr["NCPDays"] = days - item.PaidDays;
                dt.Rows.Add(dr);
                i++;
            }
            DataRow dr1 = dt.NewRow();
            dr1["UAN"] = "Total";
            dr1["Basic"] = data.MonthlySalary.Sum(x => x.Basic);
            dr1["HRA"] = data.MonthlySalary.Sum(x => x.HRA);

            dr1["EduAllowance"] = data.MonthlySalary.Sum(x => x.EduAllowance);
            dr1["TelephoneReimb"] = data.MonthlySalary.Sum(x => x.TelephoneReimb);
            dr1["SatutoryBonus"] = data.MonthlySalary.Sum(x => x.SatutoryBonus);
            dr1["CarRunningReimb"] = data.MonthlySalary.Sum(x => x.CarRunningReimb);
            dr1["OtherAllowance"] = data.MonthlySalary.Sum(x => x.OtherAllowance);

            dr1["Conveyance"] = data.MonthlySalary.Sum(x => x.Conveyance);
            dr1["Medical"] = data.MonthlySalary.Sum(x => x.Medical);
            dr1["GrossSalary"] = gross;
            dr1["PF"] = data.MonthlySalary.Sum(x => x.PF);
            dr1["VPF"] = data.MonthlySalary.Sum(x => x.VPF);
            dr1["ESI"] = data.MonthlySalary.Sum(x => x.ESI);
            dr1["TDS"] = data.MonthlySalary.Sum(x => x.TDS);
            dr1["LoanAmount"] = data.MonthlySalary.Sum(x => x.LoanAmount);
            dr1["NetDedn"] = netdend;
            dr1["Arrear"] = data.MonthlySalary.Sum(x=>x.Arrear);
            dr1["NetSalary"] = netsal;
            dr1["NCPDays"] = ncpdays;
            dt.Rows.Add(dr1);

            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "Salary-" + data.MonthYear + ".xls";
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

        public ActionResult GenratePaySlip(int id)
        {
            MonthlySalaryRepository dal = new MonthlySalaryRepository();
            EmployeeRepository emp = new EmployeeRepository();
            var data = dal.GetById(id);
            ReportDataSet rds = new ReportDataSet();
            int days = data.MonthYear.NoOfDaysInMonth();
            DataRow dr = rds.Tables["PaySlipMaster"].NewRow();
            dr["Id"] = data.Id;
            dr["EmpCode"] = data.Employee.EmpCode;
            dr["Name"] = data.Employee.Name;
            dr["UAN"] = data.Employee.UAN;
            dr["PaidDays"] = data.PaidDays;
            dr["NCPDays"] = days- data.PaidDays;
            dr["DOJ"] = Convert.ToDateTime(data.Employee.DOJ).ToString("dd-MMM-yyyy");

            dr["PayMonth"] = data.MonthYear;
            var employee = emp.GetById(data.Employee.Id);
            dr["PAN"] = employee.PAN;
            dr["Designation"] = employee.Designation.Name;
            dr["BankAccount"] = employee.BankAcc;

            dr["Basic"] = data.Basic;
            dr["HRA"] = data.HRA;
            dr["Conveyance"] = data.Conveyance;
            dr["Medical"] = data.Medical;

            dr["EduAllowance"] = data.EduAllowance;
            dr["TelephoneReimb"] = data.TelephoneReimb;
            dr["SatutoryBonus"] = data.SatutoryBonus;
            dr["CarRunningReimb"] = data.CarRunningReimb;
            dr["OtherAllowance"] = data.OtherAllowance;

            dr["GrossSalary"] = data.GrossSalary;
            dr["PF"] = data.PF;
            dr["TDS"] = data.TDS;
            dr["ESI"] = data.ESI;
            dr["Advance"] = data.LoanAmount;

            dr["NetDedn"] = data.NetDedn;
            dr["Arrear"] = data.Arrear;
            dr["NetSalary"] = data.NetSalary;

            rds.Tables["PaySlipMaster"].Rows.Add(dr);
            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\SalarySlip.rpt";
            if(data.Employee.Company.Id== 2)
            {
                repoertname = "\\Reports\\SalarySlipPTE.rpt";
            }
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

                return File(stream, "application/pdf", "SalarySlip " + data.MonthYear + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult GenerateBankLetter(int id)
        {
            MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
            var data = dal.GetById(id);

            ReportDataSet rds = new ReportDataSet();
            DataRow dr = rds.Tables["MonthlySalary"].NewRow();
            dr["Id"] = data.Id;
            dr["Cheque"] = data.Cheque;
            dr["MonthYear"] = data.MonthYear;
            dr["Date"] = Convert.ToDateTime(data.Date).ToString("dd-MMM-yyyy");
            
            rds.Tables["MonthlySalary"].Rows.Add(dr);
            foreach (var emp in data.MonthlySalary.Where(x=>x.Employee.TranType==1).OrderBy(x=>x.Employee.Name).ToList())
            {
                DataRow dr1 = rds.Tables["MonthlySalaryTrn"].NewRow();
                dr1["Id"] = data.Id;
                dr1["EmpName"] = emp.Employee.Name;
                dr1["BankAccount"] = emp.Employee.KYC.Where(x => x.DoxType == "B").Select(y => y.DocumentNumber).SingleOrDefault();
                dr1["TransAmount"] = emp.NetSalary;
                rds.Tables["MonthlySalaryTrn"].Rows.Add(dr1);
            }

            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\BankLetter.rpt";
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

                return File(stream, "application/pdf", "BankLetter " + data.MonthYear + ".pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult ExportBankLetter(int id)
        {
            MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
            var data = dal.GetById(id);

            ReportDataSet rds = new ReportDataSet();
            DataRow dr = rds.Tables["MonthlySalary"].NewRow();
            dr["Id"] = data.Id;
            dr["Cheque"] = data.Cheque;
            dr["MonthYear"] = data.MonthYear;
            dr["Date"] = Convert.ToDateTime(data.Date).ToString("dd-MMM-yyyy");

            rds.Tables["MonthlySalary"].Rows.Add(dr);
            foreach (var emp in data.MonthlySalary.Where(x => x.Employee.TranType == 1).OrderBy(x => x.Employee.Name).ToList())
            {
                DataRow dr1 = rds.Tables["MonthlySalaryTrn"].NewRow();
                dr1["Id"] = data.Id;
                dr1["EmpName"] = emp.Employee.Name;
                dr1["BankAccount"] = emp.Employee.KYC.Where(x => x.DoxType == "B").Select(y => y.DocumentNumber).SingleOrDefault();
                dr1["TransAmount"] = emp.NetSalary;
                rds.Tables["MonthlySalaryTrn"].Rows.Add(dr1);
            }

            ReportDocument rd = new ReportDocument();
            string repoertname = "\\Reports\\BankLetter.rpt";
            var reportpath = Server.MapPath(repoertname);
            rd.Load(reportpath);
            rd.SetDataSource(rds);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelRecord);
                stream.Seek(0, SeekOrigin.Begin);

                return File(stream, "application/Excel", "BulkTransfer " + data.MonthYear + ".xls");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult EmployeeSalary(int empid = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                MonthlySalaryRepository dal = new MonthlySalaryRepository();
                List<MonthlySalaryModel> model = new List<MonthlySalaryModel>();
                model = dal.GetAll().Where(x=>x.Employee.Id == empid).ToList();
                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<MonthlySalaryModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #region --- Salary Arrear ---

        public ActionResult GenrateArrearFile(int id)
        {
            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("R003"))
            {  }
            else { return View("_Security"); }

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SalaryId", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("MonthYear", typeof(string)));
            dt.Columns.Add(new DataColumn("Basic", typeof(string)));
            dt.Columns.Add(new DataColumn("HRA", typeof(string)));
            dt.Columns.Add(new DataColumn("Conveyance", typeof(string)));
            dt.Columns.Add(new DataColumn("Medical", typeof(string)));
            dt.Columns.Add(new DataColumn("PF", typeof(string)));
            dt.Columns.Add(new DataColumn("ESI", typeof(string)));

            GridView gv = new GridView();
            MonthlySalaryMasterRepository dal = new MonthlySalaryMasterRepository();
            var data = dal.GetById(id);
            foreach (var item in data.MonthlySalary.OrderBy(x => x.Employee.Name))
            {
                DataRow dr = dt.NewRow();
                dr["SalaryId"] = item.Id;
                dr["EmpCode"] = item.Employee.EmpCode;
                dr["Name"] = item.Employee.Name;
                dr["MonthYear"] = "";
                dr["Basic"] = 0;
                dr["HRA"] = 0;
                dr["Conveyance"] = 0;
                dr["Medical"] = 0;
                dr["PF"] = 0;
                dr["ESI"] = 0;
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "SalaryArrear.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            string error = string.Empty;

            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("R004"))
            { }
            else { return Json(Convert.ToString(error == "ER" ? "Arrear Updated" : "Hi, You are not authorised to do this action."), JsonRequestBehavior.AllowGet); }


            var dirPath = Server.MapPath(Url.Content("~/Upload/Temp/"));
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dirPath);
            foreach (FileInfo fi in directory.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
           
            string _imgname = string.Empty;
            string imagepath = string.Empty;
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    System.Web.HttpPostedFile hpf = hfc[iCnt];

                    if (hpf.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(hpf.FileName);
                        var _ext = Path.GetExtension(hpf.FileName);

                        if (_ext.ToLower() == ".csv")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/Temp/")) + hpf.FileName;
                            var path = _comPath;
                            hpf.SaveAs(path);

                            try
                            {
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;
                                MonthlySalaryDAL dal = new MonthlySalaryDAL();
                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        IMonthlySalary model = dal.GetById(Convert.ToInt32(x[0].Trim()));
                                        ISalaryArrear bl = new SalaryArrear();
                                        bl.MonthYear = Convert.ToString(x[3].Trim());
                                        bl.Basic = Convert.ToDouble(x[4].Trim());
                                        bl.HRA = Convert.ToDouble(x[5].Trim());
                                        bl.Conveyance = Convert.ToDouble(x[6].Trim());
                                        bl.Medical = Convert.ToDouble(x[7].Trim());
                                        bl.PF = Convert.ToDouble(x[8].Trim());
                                        bl.ESI = Convert.ToDouble(x[9].Trim());
                                        model.SalaryArrear.Add(bl);
                                        dal.InsertOrUpdate(model);

                                    }
                                    catch (Exception ex) { error = error + "," + (sr - 1).ToString() + ex.Message; }
                                }

                            }
                            catch (Exception ex) { return Json(Convert.ToString(ex.Message), JsonRequestBehavior.AllowGet); }
                        }
                        else
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Uploads/TempCV/")) + hpf.FileName;
                            var path = _comPath;
                            hpf.SaveAs(path);
                        }
                    }
                }
            }
            return Json(Convert.ToString(error == "" ? "Arrear Updated" : "Error Found in Record" + error), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ArrearDetails(int id = 0)
        {
            try
            {
                MonthlySalaryRepository dal = new MonthlySalaryRepository();
                List<SalaryArrearModel> model = new List<SalaryArrearModel>();
                model = dal.GetAllArrear(id).ToList();

                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateArrear(SalaryArrearModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlySalaryRepository dal = new MonthlySalaryRepository();
                

                if (User.IsInRole("Admin"))
                {
                    dal.EditArrear(model);
                    model = dal.GetByIdArrear(model.Id);
                }
                else if (Models.Admin.UserRightRepository
                    .RightList(User.Identity.Name).Contains("M007"))
                {
                    dal.EditArrear(model);
                    model = dal.GetByIdArrear(model.Id);
                }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddArrear(SalaryArrearModel model, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlySalaryRepository dal = new MonthlySalaryRepository();

                if (User.IsInRole("Admin")) { dal.InsertArrear(model,id); }
                else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("R002")) { dal.InsertArrear(model,id); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteArrear(SalaryArrearModel model)
        {
            if (User.IsInRole("Admin")) {  }
            else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                MonthlySalaryRepository dal = new MonthlySalaryRepository();
                var data = dal.DeleteArrear(model.Id);

                return Json(new { Result = "OK", Record = data });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        #endregion

    }
}