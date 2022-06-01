using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebPayroll.Models.Master;
using WebPayroll.Models.Transaction;

namespace WebPayroll.Controllers.Master
{
    [Authorize]
    public class SalaryMasterController : Controller
    {
        static List<SalaryMasterModel> salaryList = new List<SalaryMasterModel>();
        // GET: SalaryMaster
        public ActionResult Index()
        {
            BranchRepository dal = new BranchRepository();
            ViewBag.branch = new SelectList(dal.GetAll().OrderBy(x => x.Name), "Id", "Name");

            ViewBag.SearchFields = new SelectList(new[] {
                new SelectListItem { Text = "Name", Value = "1" },
            }, "Value", "Text");

            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M014")) { return View(); }
            else { return View("_Security"); }
        }

        [HttpPost]
        public JsonResult List(int branch = 0, int field = 0, string search = null, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                List<SalaryMasterModel> model = new List<SalaryMasterModel>();
                model = branch!=0
                        ?dal.GetAll().Where(x=>x.Employee.Branch.Id ==branch).ToList()
                        :dal.GetAll().ToList();

                if (!string.IsNullOrEmpty(search))
                {
                    switch (field)
                    {
                        case 1:
                            model = model.Where(x => (x.Employee.Name + "").ToLower().Contains(search.Trim().ToLower())).ToList();
                            break;
                    }
                }

                int count = model.Count;
                List<SalaryMasterModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Create(SalaryMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                SalaryMasterRepository dal = new SalaryMasterRepository();
                dal.Insert(model);
                return Json(new { Result = "OK", Record = model });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(SalaryMasterModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { Result = "ERROR", Message = "Form is not valid! Please correct it and try again." });
                }
                SalaryMasterRepository dal = new SalaryMasterRepository();
               
                if (User.IsInRole("Admin"))
                {
                    dal.Edit(model);
                    model = dal.GetById(model.Id);
                }
                else if (Models.Admin.UserRightRepository
                    .RightList(User.Identity.Name).Contains("M015"))
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

        public JsonResult GetDetails(int b, string s, int SF)                       //fetching details of the selected user and passing to the javascript function  
        {
            try
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                var data = dal.GetByBranch(b).ToList();
                if (!string.IsNullOrEmpty(s))
                {
                    switch (SF)
                    {
                        case 1:
                            data = data.Where(x => (x.Employee.Name + "").ToLower().Contains(s.Trim().ToLower())).ToList();
                            break;
                        case 2:
                            data = data.Where(x => (x.Employee.UAN + "").ToLower().Contains(s.Trim().ToLower())).ToList();
                            break;
                        case 3:
                            data = data.Where(x => (x.Employee.ESIIP + "").ToLower().Contains(s.Trim().ToLower())).ToList();
                            break;
                    }
                }
                SalaryMasterModel sal = data.Take(1).SingleOrDefault();
                if (sal != null)
                {
                    return Json(sal, JsonRequestBehavior.AllowGet);
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
                SalaryMasterRepository dal = new SalaryMasterRepository();
                var data = dal.GetAll().Where(x => x.Employee.Id == empid).ToList();
                
                SalaryMasterModel sal = data.Take(1).SingleOrDefault();
                if (sal != null)
                {
                    return Json(sal, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SalaryRevision(SalaryMasterModel objSalary)
        {
            try
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                dal.SalaryRevision(objSalary);
                return Json(new { Result = "OK", Record = objSalary });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }


        [HttpPost]
        public JsonResult EmployeePastSalary(int empid = 0, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                SalaryRevisionRepository dal = new SalaryRevisionRepository();
                List<SalaryRevisionModel> model = new List<SalaryRevisionModel>();
                model = dal.GetAll().Where(x => x.Employee.Id == empid).ToList();
                int count = model.Count;
                model = model.OrderByDescending(x => x.Id).ToList();
                List<SalaryRevisionModel> Model1 = model.Skip(jtStartIndex).Take(jtPageSize).ToList();
                return Json(new { Result = "OK", Records = Model1, TotalRecordCount = count });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Error", Message = ex.Message });
            }
        }

        #region --- Update Bulk Salary

        public ActionResult BulkSalary()
        {
            CompanyRepository comp = new CompanyRepository();
            ViewBag.comp = new SelectList(comp.GetAll().OrderBy(x => x.Name), "Id", "Name");

            BranchRepository bran = new BranchRepository();
            ViewBag.branch = new SelectList(bran.GetAll().OrderBy(x => x.Name), "Id", "Name");

            if (User.IsInRole("Admin")) { return View(); }
            else if (Models.Admin.UserRightRepository.RightList(User.Identity.Name).Contains("M016")) { return View(); }
            else { return View("_Security"); }
        }

        public ActionResult GenrateFile(int compid,int branid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SalaryId", typeof(string)));
            dt.Columns.Add(new DataColumn("EmpCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Basic", typeof(string)));
            dt.Columns.Add(new DataColumn("DA", typeof(string)));
            dt.Columns.Add(new DataColumn("HRA", typeof(string)));
            dt.Columns.Add(new DataColumn("Conveyance", typeof(string)));
            dt.Columns.Add(new DataColumn("Medical", typeof(string)));
            dt.Columns.Add(new DataColumn("VPF", typeof(string)));
            dt.Columns.Add(new DataColumn("TDS", typeof(string)));

            GridView gv = new GridView();
            SalaryMasterRepository dal = new SalaryMasterRepository();
            var data = dal.GetAll(compid, branid).ToList();
            int i = 1;
            foreach (var item in data.OrderBy(x=>x.Employee.Name))
            {
                DataRow dr = dt.NewRow();
                dr["SalaryId"] = item.Id;
                dr["EmpCode"] = item.Employee.EmpCode;
                dr["Name"] = item.Employee.Name;
                dr["Basic"] = item.Basic;
                dr["DA"] = item.DA;
                dr["HRA"] = item.HRA;
                dr["Conveyance"] = item.Conveyance;
                dr["Medical"] = item.Medical;
                dr["VPF"] = item.VPF;
                dr["TDS"] = item.TDS;
                dt.Rows.Add(dr);
                i++;
            }

            gv.DataSource = dt;
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            string fileName = "Salary.xls";
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
                                SalaryMasterRepository dal = new SalaryMasterRepository();
                                foreach (var x in csv.Skip(1))
                                {
                                    try
                                    {
                                        SalaryMasterModel bl = dal.GetById(Convert.ToInt32(x[0].Trim()));
                                        bl.EmpId = bl.Employee.Id;
                                        bl.EmpCode = x[1].Trim();
                                        bl.Basic = Convert.ToDouble(x[3].Trim());
                                        bl.DA = Convert.ToDouble(x[4].Trim());
                                        bl.HRA = Convert.ToDouble(x[5].Trim());
                                        bl.Conveyance = Convert.ToDouble(x[6].Trim());
                                        bl.Medical = Convert.ToDouble(x[7].Trim());
                                        bl.VPF = Convert.ToDouble(x[8].Trim());
                                        bl.TDS = Convert.ToDouble(x[9].Trim());

                                        if (bl.IsPFExempted == "N")
                                        {
                                            if (bl.IsPFCelling == "Y")
                                            {
                                                bl.PF = bl.Basic > bl.Employee.StatutoryCode.PFCelling
                                                    ? 1800 : Math.Round((bl.Basic * bl.Employee.StatutoryCode.PFCont) / 100);
                                            }
                                            else
                                            {
                                                bl.PF = Math.Round((bl.Basic * bl.Employee.StatutoryCode.PFCont) / 100);
                                            }
                                        }
                                        else
                                        {
                                            bl.PF = 0;
                                        }

                                        if (bl.IsESIExempted == "N")
                                        {
                                            bl.ESI = Math.Ceiling((bl.GrossSalary * bl.Employee.StatutoryCode.ESICont) / 100);
                                        }
                                        else
                                        {
                                            bl.ESI = 0;
                                        }

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
        public JsonResult BulkSalaryList()
        {
            try
            {
                List<SalaryMasterModel> model = new List<SalaryMasterModel>();
                model = salaryList.ToList();
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
            if (salaryList.Count == 0)
            {
                message = "Please select file to upload";
            }
            else
            {
                SalaryMasterRepository dal = new SalaryMasterRepository();
                foreach (var obj in salaryList)
                {
                    dal.Edit(obj);
                }
                salaryList.Clear();
                message = "Record Uploaded Successfully";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}