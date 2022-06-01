using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPayroll.Areas.LMS.Models;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Models.Master;

namespace WebPayroll.Controllers.Master
{
    [Authorize]
    public class EmployeeController : Controller
    {
        static List<EmployeeModel> emplist = new List<EmployeeModel>();
        static List<EmployeeKYCModel> draftKYC = new List<EmployeeKYCModel>();
        static List<EmployeeModel> draftEmpList = new List<EmployeeModel>();

        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "Name", Value = "1" },
                new SelectListItem { Text = "UAN", Value = "2" },
                new SelectListItem { Text = "ESI IP", Value = "4" },
            }, "Value", "Text");

            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M006")) { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int status = 0, int branch = 0, int field = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                List<EmployeeModel> model = new List<EmployeeModel>();
                model = dal.GetAll().ToList();
                if (branch != 0)
                {
                    model = model.Where(x => x.Branch.Id == branch && x.Status == 1).ToList();
                }
                
                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => (x.Name +"").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                        case 2:
                            model = model.Where(x => (x.UAN + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                        case 3:
                            model = model.Where(x => (x.ESIIP + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                    }
                }
                int count = model.Count;
                emplist = model.ToList();
                model = model.OrderByDescending(x => x.EmpCode).ToList();
                List<EmployeeModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                EmployeeRepository dal = new EmployeeRepository();
                SalaryMasterRepository saldal = new SalaryMasterRepository();

                if (User.IsInRole("Admin")) { dal.Insert(model); }
                else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M007")) { dal.Insert(model); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(EmployeeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                EmployeeRepository dal = new EmployeeRepository();

                if (User.IsInRole("Admin")) {
                    dal.Edit(model);
                    model = dal.GetById(model.Id);
                }
                else if (Models.Admin.UserRightRepository
                    .RightList(User.Identity.Name).Contains("M007"))
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

        [HttpPost]
        public JsonResult GetEmployeeOptions()
        {
            List<EmployeeModel> model = new List<EmployeeModel>();
            model.Add(new EmployeeModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                var list = dal.GetAll()
                                .Select(c => new { DisplayText = c.Name, Value = c.Id });
                return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

       
        [HttpPost]
        public JsonResult GetEmployeeByBran(int branid)
        {
            List<EmployeeModel> model = new List<EmployeeModel>();
            model.Add(new EmployeeModel { Id = 0, Name = "* Select *" });
            var data = model.Select(c => new { DisplayText = c.Name, Value = c.Id });
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                if(branid !=0)
                {
                    var list = dal.GetAll().Where(x=>x.Branch.Id==branid)
                    .Select(c => new { DisplayText = c.Name, Value = c.Id });
                    return Json(new { Result = "OK", Options = list.Concat(data).OrderBy(x => x.DisplayText) });
                }
                else
                {
                    return Json(new { Result = "OK", Options = data.OrderBy(x => x.DisplayText) });
                }
                
                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetEmpByBran(int branid)
        {
            EmployeeRepository dal = new EmployeeRepository();
            List<EmployeeModel> model = new List<EmployeeModel>();
            if (branid != 0)
            {
                model = dal.GetAll().Where(x => x.Branch.Id == branid).ToList();
            }
            return Json(new { Result = model.OrderBy(x => x.Name) });
        }

        [HttpPost]
        public JsonResult GetEmployeeInfo(int empid)
        {
            EmployeeRepository dal = new EmployeeRepository();
            List<EmployeeModel> model = new List<EmployeeModel>();
            model.Add(dal.GetById(empid));
            return Json(new { Result = model });
        }

        public ActionResult EmployeeFullDetails()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "Name", Value = "1" },
                new SelectListItem { Text = "UAN", Value = "2" },
                new SelectListItem { Text = "ESI IP", Value = "4" },
            }, "Value", "Text");

            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M009")) { return View(); }
            else { return View("_Security"); }
        }

        public JsonResult GetDetails(int b,string s,int SF)                       //fetching details of the selected user and passing to the javascript function  
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                var data = dal.GetByBranch(b).ToList();
                if (!string.IsNullOrEmpty(s))
                {
                    switch (SF)
                    {
                        case 1:
                            data = data.Where(x => (x.Name + "").ToLower().Contains(s.Trim().ToLower())).ToList();
                            break;
                        case 2:
                            data = data.Where(x => (x.UAN + "").ToLower().Contains(s.Trim().ToLower())).ToList();
                            break;
                        case 3:
                            data = data.Where(x => (x.ESIIP + "").ToLower().Contains(s.Trim().ToLower())).ToList();
                            break;
                    }
                }
                EmployeeModel emp = data.Take(1).SingleOrDefault();
                if (emp!= null)
                {
                    return Json(emp, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetDetailsById(int empid)                       //fetching details of the selected user and passing to the javascript function  
        {
            try
            {
                EmployeeRepository dal = new EmployeeRepository();
                EmployeeModel emp = dal.GetById(empid);
                if (emp != null)
                {
                    return Json(emp, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region ---- KYC ----

        public JsonResult GetKYC(int empid)
        {
            try
            {
                EmployeeKYCRepository dal = new EmployeeKYCRepository();
                List<EmployeeKYCModel> model = new List<EmployeeKYCModel>();
               

                if (User.IsInRole("Admin"))
                {
                    model = dal.GetAll(empid).ToList();
                }
                else if (Models.Admin.UserRightRepository
                    .RightList(User.Identity.Name).Contains("M011"))
                {
                    model = dal.GetAll(empid).ToList();
                }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

                int count = model.Count;
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddKYC(EmployeeKYCModel model, int empid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                EmployeeKYCRepository dal = new EmployeeKYCRepository();

                if (User.IsInRole("Admin")) { dal.Insert(model, empid); }
                else if (Models.Admin.UserRightRepository.
                    RightList(User.Identity.Name).Contains("M010")) { dal.Insert(model, empid); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

               
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateKYC(EmployeeKYCModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                EmployeeKYCRepository dal = new EmployeeKYCRepository();
               

                if (User.IsInRole("Admin")) { dal.Edit(model); }
                else if (Models.Admin.UserRightRepository.
                    RightList(User.Identity.Name).Contains("M010")) { dal.Edit(model); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetKYCdoxtype()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("A", "Aadhar Card");
            values.Add("P", "Pan Card");
            values.Add("B", "Bank A/c");
            values.Add("T", "Passport");
            values.Add("E", "Election Card");
            values.Add("R", "Ration Card");
            values.Add("D", "Driving License");
            values.Add("N", "National Population Register");
            values.Add("H", "AADHAAR Enrolment Number");
            try
            {
                var list = values.Select(c => new { DisplayText = c.Value, Value = c.Key });
                return Json(new { Result = "OK", Options = list });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenrateFile()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("DoxType", typeof(string)));
            dt.Columns.Add(new DataColumn("NameonDox", typeof(string)));
            dt.Columns.Add(new DataColumn("DocumentNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("Other", typeof(string)));
            dt.Columns.Add(new DataColumn("IssueDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Exipiry", typeof(string)));
            dt.Columns.Add(new DataColumn("Place", typeof(string)));

            GridView gv = new GridView();
            foreach (var item in emplist.OrderBy(x => x.Name))
            {
                DataRow dr = dt.NewRow();
                dr["Id"] = item.Id;
                dr["Code"] = item.EmpCode;
                dr["Name"] = item.Name;
                dr["DoxType"] = "";
                dr["NameonDox"] = "";
                dr["DocumentNumber"] ="";
                dr["Other"] = "";
                dr["IssueDate"] = "";
                dr["Exipiry"] = "";
                dr["Place"] = "";
                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "UpdateKYC.xls";
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

        public ActionResult BulkKYC()
        {
            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M013")) { return View(); }
            else { return View("_Security"); }
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
                                draftKYC.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {

                                        EmployeeKYCModel bl = new EmployeeKYCModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.EmpId = Convert.ToInt32(x[0].Trim());
                                        bl.DoxType = x[3].Trim();
                                        bl.NameonDox = x[4].Trim();
                                        bl.DocumentNumber = x[5].Trim();
                                        bl.Other = x[6].Trim();
                                        if (bl.DoxType == "T")
                                        {
                                            if (x[7].Trim() != "")
                                                bl.IssueDate = Convert.ToDateTime(x[7].Trim());
                                            if (x[8].Trim() != "")
                                                bl.Exipiry = Convert.ToDateTime(x[8].Trim());
                                            bl.Place = x[9].Trim();
                                        }
                                        draftKYC.Add(bl);
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
            return Json(Convert.ToString("Error Found in Record " + error), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BulkList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<EmployeeKYCModel> model = new List<EmployeeKYCModel>();
                model = draftKYC.ToList();
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
            EmployeeDAL dal = new EmployeeDAL();
            string message = "";
            if (draftKYC.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                foreach (var item in draftKYC.GroupBy(x => x.EmpId))
                {
                    IEmployee emp = dal.GetById(item.Key);
                    foreach (var obj in item)
                    {
                        IEmployeeKYC bl = new EmployeeKYC();
                        bl.DoxType = obj.DoxType.ToUpper();
                        bl.DocumentNumber = obj.DocumentNumber.ToUpper();
                        bl.NameonDox = obj.NameonDox;
                        bl.Other = obj.Other;
                        bl.IssueDate = obj.IssueDate;
                        bl.Exipiry = obj.Exipiry;
                        bl.Place = obj.Place;
                        emp.KYC.Add(bl);
                    }
                    dal.InsertOrUpdate(emp);
                }
                draftKYC.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadKycDox(int kycid)
        {
            if (User.IsInRole("Admin")) { }
            else if (Models.Admin.UserRightRepository.
                RightList(User.Identity.Name).Contains("M012")) { }
            else {
                return Json(Convert.ToString("Hi, You are not Authorized to do this action."), JsonRequestBehavior.AllowGet);
            }


                var dirPath = Server.MapPath(Url.Content("~/Upload/KYCDox/"));
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

                        if (_ext.ToLower() == ".pdf")
                        {
                            var _comPath = Server.MapPath(Url.Content("~/Upload/KYCDox/")) + kycid + _ext.ToLower();
                            var path = _comPath;
                            hpf.SaveAs(path);
                        }
                    }
                }
                return Json(Convert.ToString("File Uploaded Successfully"), JsonRequestBehavior.AllowGet);
            }
            return Json(Convert.ToString("Error Found in Record " + error), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ---- Bulk Employee Upload ---

        public ActionResult BulkEmployee()
        {
            CompanyRepository comp = new CompanyRepository();
            ViewBag.comp = new SelectList(comp.GetAll().OrderBy(x => x.Name), "Id", "Name");

            BranchRepository bran = new BranchRepository();
            ViewBag.branch = new SelectList(bran.GetAll().OrderBy(x => x.Name), "Id", "Name");

            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M008")) { return View(); }
            else { return View("_Security"); }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadEmpFile(int CompId, int BranchId)
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
                                draftEmpList.Clear();
                                var csv = from line in System.IO.File.ReadLines(_comPath)
                                          select (line.Split(',')).ToArray();
                                int sr = 1;

                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        EmployeeModel bl = new EmployeeModel();
                                        bl.Id = sr;
                                        sr++;
                                        bl.CompId = CompId;
                                        bl.BranchId = BranchId;
                                        bl.EmpCode = x[0].Trim();
                                        bl.Name = x[1].Trim();
                                        bl.Gender = x[2].Trim();
                                        bl.MarritalStatus = x[3].Trim();
                                        if (x[4].Trim() != "") bl.DOB = Convert.ToDateTime(x[4].Trim());
                                        bl.FatherOrHusbandName = x[5].Trim();
                                        bl.FNHFlag = x[6].Trim();
                                        bl.UAN = x[7].Trim();
                                        bl.ESIIP = x[8].Trim();
                                        if (x[9].Trim() != "") bl.DOJ = Convert.ToDateTime(x[9].Trim());
                                        bl.StatuId = Convert.ToInt32(x[10].Trim());
                                        bl.DeptId = Convert.ToInt32(x[11].Trim());
                                        bl.DesigId = Convert.ToInt32(x[12].Trim());
                                        bl.Status = 1;
                                        bl.TranType = 1;
                                        draftEmpList.Add(bl);
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
            return Json(Convert.ToString(error==""?"No Error Found in Record":"Error Found in Record" + error), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BulkEmpList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                List<EmployeeModel> model = new List<EmployeeModel>();
                model = draftEmpList.ToList();
                int count = model.Count;
                return Json(new { Result = "OK", Records = model, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        public JsonResult SaveEmpBulk()
        {
            
            string message = "";
            if (draftEmpList.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                EmployeeRepository dal = new EmployeeRepository();
                foreach (var obj in draftEmpList)
                {
                    dal.Insert(obj);
                }
                draftEmpList.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region --- Past Woring ----

        public JsonResult GetPastWorking(int empid)
        {
            try
            {
                EmployeePastWorkingRepository dal = new EmployeePastWorkingRepository();
                List<EmployeePastWorkingModel> model = new List<EmployeePastWorkingModel>();


                if (User.IsInRole("Admin"))
                {
                    model = dal.GetAll(empid).ToList();
                }
                else if (Models.Admin.UserRightRepository
                    .RightList(User.Identity.Name).Contains("M011"))
                {
                    model = dal.GetAll(empid).ToList();
                }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }

                int count = model.Count;
                return Json(new { Result = "OK", Records = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AddPastWorking(EmployeePastWorkingModel model, int empid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                EmployeePastWorkingRepository dal = new EmployeePastWorkingRepository();

                if (User.IsInRole("Admin")) { dal.Insert(model, empid); }
                else if (Models.Admin.UserRightRepository.
                    RightList(User.Identity.Name).Contains("M010")) { dal.Insert(model, empid); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }


                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult UpdatePastWoring(EmployeePastWorkingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }

                EmployeePastWorkingRepository dal = new EmployeePastWorkingRepository();


                if (User.IsInRole("Admin")) { dal.Edit(model); }
                else if (Models.Admin.UserRightRepository.
                    RightList(User.Identity.Name).Contains("M010")) { dal.Edit(model); }
                else { return Json(new { Result = "Error", Message = "Hi, You are not Authorized to do this action." }); }
                return Json(new { Result = "OK", Record = model });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }
        #endregion

        public ActionResult EmployeeHeadCount()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult EmployeeHeadCountChart()
        {
            EmployeeRepository dal = new EmployeeRepository();
            List<EmployeeModel> model = dal.GetAll().Where(x => x.Status == 1).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Dept", System.Type.GetType("System.String"));
            dt.Columns.Add("Emp", System.Type.GetType("System.Double"));

            List<object> iData = new List<object>();

            foreach (var item in model.GroupBy(x => x.Department))
            {
                DataRow dr = dt.NewRow();
                dr["Dept"] = item.Key.Name;
                dr["Emp"] = item.Count();
                dt.Rows.Add(dr);
            }
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenderSplit()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult GenderSplitChart()
        {
            EmployeeRepository dal = new EmployeeRepository();
            List<EmployeeModel> model = dal.GetAll().Where(x => x.Status == 1).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("Gender", System.Type.GetType("System.String"));
            dt.Columns.Add("Count", System.Type.GetType("System.Double"));

            List<object> iData = new List<object>();

            foreach (var item in model.GroupBy(x => x.Gender))
            {
                DataRow dr = dt.NewRow();
                dr["Gender"] = item.Key=="M"?"Male":"Female";
                dr["Count"] = item.Count();
                dt.Rows.Add(dr);
            }
            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }

            return Json(iData.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TimingList(int EmpCode, int month, int year)
        {
            MonthlyAttendanceRepository dal = new MonthlyAttendanceRepository();
            List<MonthlyAttendanceModel> model = dal.GetByEmp(EmpCode).Where(x => x.Month == month && x.Year == year).ToList();

            var dates = new List<DateTime>();

            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            ViewBag.cal = dates;

            return PartialView(model.OrderBy(x => x.Days));
        }
    }
}