using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Master
{
    public class CompanyDAL: Common.CommonDAL<ICompany>
    {
    }

    public class BranchDAL : Common.CommonDAL<IBranch>
    {

    }

    public class DepartmentDAL : Common.CommonDAL<IDepartment>
    {

    }

    public class DesignationDAL : Common.CommonDAL<IDesignation>
    {

    }
}