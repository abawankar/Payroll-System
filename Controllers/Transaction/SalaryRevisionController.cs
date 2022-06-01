using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPayroll.DAL.Master;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Models.Admin;
using WebPayroll.Models.Master;
using WebPayroll.Models.Transaction;

namespace WebPayroll.Controllers.Transaction
{
    public class SalaryRevisionController : Controller
    {
        static List<TempSalaryRevModel> salaryList = new List<TempSalaryRevModel>();
        // GET: SalaryRevision
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.emp = new SelectList(Enumerable.Empty<SelectListItem>());

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 4)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.Months = new SelectList(data, "Value", "Text");

            if (User.IsInRole("Admin") || AppsUserRepository.RightList(User.Identity.Name).Contains("T001"))
            { return View(); }
            else { return View("_Security"); }
        }

        public ActionResult BulkRevision()
        {
            CompanyRepository comp = new CompanyRepository();
            ViewBag.comp = new SelectList(comp.GetAll().OrderBy(x => x.Name), "Id", "Name");

            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            var now = DateTimeOffset.Now;
            var data = Enumerable.Range(0, 5)
                .Select(i => new SelectListItem { Value = now.AddMonths(-i).ToString("MM/yyyy"), Text = now.AddMonths(-i).ToString("MM/yyyy") });
            ViewBag.Months = new SelectList(data, "Value", "Text");

            return View();
        }

        public ActionResult GenrateFile(int compid, int branid, string month)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SalaryId", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpId", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("RevisionMonth", typeof(string)));
            dt.Columns.Add(new DataColumn("Basic", typeof(string)));
            dt.Columns.Add(new DataColumn("HRA", typeof(string)));
            dt.Columns.Add(new DataColumn("Conveyance", typeof(string)));
            dt.Columns.Add(new DataColumn("Medical", typeof(string)));
            dt.Columns.Add(new DataColumn("PF", typeof(string)));
            dt.Columns.Add(new DataColumn("ESI", typeof(string)));
            dt.Columns.Add(new DataColumn("ArrearMonth", typeof(string)));

            GridView gv = new GridView();
            SalaryMasterRepository dal = new SalaryMasterRepository();
            var data = dal.GetAll(compid, branid).ToList();
            int i = 1;
            foreach (var item in data.OrderBy(x => x.Employee.Name))
            {
                DataRow dr = dt.NewRow();
                dr["SalaryId"] = item.Id;
                dr["EmpId"] = item.Employee.Id;
                dr["EmpCode"] = item.Employee.EmpCode;
                dr["Name"] = item.Employee.Name;
                dr["Basic"] = 0;
                dr["HRA"] = 0;
                dr["Conveyance"] = 0;
                dr["Medical"] = 0;
                dr["PF"] = 0;
                dr["ESI"] = 0;
                dr["RevisionMonth"] = "~" + month;
                dr["ArrearMonth"] = 0;
                dt.Rows.Add(dr);
                i++;
            }

            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "RevisedSalary.xls";
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("BulkRevision");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadFile()
        {
            var dirPath = Server.MapPath(Url.Content("~/Upload/Temp/"));
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(dirPath);
            foreach (FileInfo fi in directory.GetFiles())
            {
                fi.IsReadOnly = false;
                fi.Delete();
            }
            string error = string.Empty;
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
                                salaryList.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;
                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        TempSalaryRevModel bl = new TempSalaryRevModel();
                                        bl.SalaryId = Convert.ToInt32(x[0].Trim());
                                        bl.EmpId = Convert.ToInt32(x[1].Trim());
                                        bl.EmpCode = x[2].Trim();
                                        bl.EmpName = x[3].Trim();
                                        bl.MonthYear = x[4].Trim().Replace("~", "");
                                        bl.Basic = Convert.ToDouble(x[5].Trim());
                                        bl.HRA = Convert.ToDouble(x[6].Trim());
                                        bl.Conveyance = Convert.ToDouble(x[7].Trim());
                                        bl.Medical = Convert.ToDouble(x[8].Trim());
                                        bl.PF = Convert.ToDouble(x[9].Trim());
                                        bl.ESI = Convert.ToDouble(x[10].Trim());
                                        bl.ArrearMonth = Convert.ToInt32(x[11].Trim());
                                        salaryList.Add(bl);
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
            return Json(Convert.ToString(error == "" ? "No Error Found in Record" : "Error Found in Record" + error), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BulkReviseSalaryList()
        {
            try
            {
                List<TempSalaryRevModel> model = new List<TempSalaryRevModel>();
                model = salaryList.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult SaveBulk()
        {

            string message = "";
            if (salaryList.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                SalaryMasterDAL dal = new SalaryMasterDAL();
                SalaryRevisionDAL revdal = new SalaryRevisionDAL();
                ISalaryRevision revBl = new SalaryRevision();
                MonthlySalaryDAL mdal = new MonthlySalaryDAL();

                SalaryArrearDAL aDal = new SalaryArrearDAL();

                foreach (var obj in salaryList)
                {
                    ISalaryMaster bl = dal.GetById(obj.SalaryId);
                    //Old Salary
                    revBl.MonthYear = obj.MonthYear;
                    revBl.Basic = bl.Basic;
                    revBl.DA = bl.DA;
                    revBl.HRA = bl.HRA;
                    revBl.Conveyance = bl.Conveyance;
                    revBl.Medical = bl.Medical;
                    revBl.EduAllowance = bl.EduAllowance;
                    revBl.TelephoneReimb = bl.TelephoneReimb;
                    revBl.CarRunningReimb = bl.CarRunningReimb;
                    revBl.SatutoryBonus = bl.SatutoryBonus;
                    revBl.OtherAllowance = bl.OtherAllowance;
                    revBl.PF = bl.PF;
                    revBl.VPF = bl.VPF;
                    revBl.ESI = bl.ESI;
                    revBl.TDS = bl.TDS;
                    revBl.Employee = new Employee { Id = obj.EmpId };
                    revdal.InsertOrUpdate(revBl);

                    //Revised Salary
                    bl.Basic = obj.Basic;
                    bl.HRA  = obj.HRA;
                    bl.Conveyance = obj.Conveyance;
                    bl.Medical = obj.Medical;
                    bl.PF = obj.PF;
                    bl.ESI = obj.ESI;
                    dal.InsertOrUpdate(bl);

                    //Calculate Arrear
                    for (int i = 0; i < obj.ArrearMonth; i++)
                    {
                        try
                        {
                            DateTime dt = new DateTime(Convert.ToInt32(obj.MonthYear.Substring(3, 4)), Convert.ToInt32(obj.MonthYear.Substring(0, 2)), 1);
                            DateTime newdt = dt.AddMonths(i);
                            int year = newdt.Year;
                            int month = newdt.Month;
                            int days = System.DateTime.DaysInMonth(year, month);
                            string monthyear = month.ToString().PadLeft(2, '0') + "/" + year.ToString();

                            IMonthlySalary msal = mdal.GetEmpMonth(obj.EmpId, monthyear).SingleOrDefault();
                            double basicArrear = Math.Round((obj.Basic / days) * msal.PaidDays) - msal.Basic;
                            double hraArrear = Math.Round((obj.HRA / days) * msal.PaidDays) - msal.HRA;
                            double coneyvanceArrear = Math.Round((obj.Conveyance / days) * msal.PaidDays) - msal.Conveyance;
                            double medicalArrear = Math.Round((obj.Medical / days) * msal.PaidDays) - msal.Medical;
                            double pfArrear = Math.Round((basicArrear * bl.Employee.StatutoryCode.PFCont) / 100);
                            double esiArrear = 0;
                            double grossArrear = basicArrear + hraArrear + coneyvanceArrear + medicalArrear;
                            if (obj.ESI != 0)
                            {
                                esiArrear = Math.Ceiling((grossArrear * bl.Employee.StatutoryCode.ESICont) / 100);
                            }
                            double netArrear = grossArrear - pfArrear - esiArrear;

                            ISalaryArrear sbl = new SalaryArrear();
                            sbl.Basic = basicArrear;
                            sbl.HRA = hraArrear;
                            sbl.Conveyance = coneyvanceArrear;
                            sbl.Medical = medicalArrear;
                            sbl.PF = pfArrear;
                            sbl.ESI = esiArrear;
                            sbl.MonthYear = monthyear;
                            sbl.EmpId = obj.EmpId;
                            aDal.InsertOrUpdate(sbl);
                        }
                        catch { };
                    }
                }
                salaryList.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}