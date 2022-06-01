using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPayroll.DAL.Common;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Controllers.Admin
{
    public class DBController : Controller
    {
        // GET: DB
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateDataBase()
        {
            string strCon = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            //CreateDB.CreateDatabase(strCon);

            CompanyDAL comp = new CompanyDAL();
            ICompany compbl = new Company();
            compbl.Code = "ASPL";
            compbl.Name = "APRIL SOURCING PVT LTD";
            comp.InsertOrUpdate(compbl);

            BranchDAL dal1 = new BranchDAL();
            IBranch bl1 = new Branch();
            bl1.Code = "B001";
            bl1.Name = "GURGAON";
            dal1.InsertOrUpdate(bl1);

            DepartmentDAL dal2 = new DepartmentDAL();
            IDepartment bl2 = new Department();
            bl2.Name = "ACCOUNT";
            dal2.InsertOrUpdate(bl2);

            DesignationDAL dal3 = new DesignationDAL();
            IDesignation bl3 = new Designation();
            bl3.Name = "ACCOUNTANT";
            dal3.InsertOrUpdate(bl3);


            ViewData["message"] = "Database Created Successfully";
            return View();
        }
    }
}