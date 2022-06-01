using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Transaction
{
    public class MonthlySalaryMasterDAL : Common.CommonDAL<IMonthlySalaryMaster>
    {
        public bool DeleteDraftReturn(int branchid, string monthyear)
        {
            bool flag = false;
            ISession session1 = NHibernateHelper.OpenSession();
            try
            {
                var q1 = "delete DraftMonthlySalary where BranchId =" + branchid + " and MontYear='" + monthyear + "'";
                var result1 = session1.CreateSQLQuery(q1).ExecuteUpdate();
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            return flag;
        }
    }
    public class MonthlySalaryDAL:Common.CommonDAL<IMonthlySalary>
    {
        public static IList<ISalaryArrear> GetSalaryArrear(int empid)
        {
            try
            {
                var query = "select * from SalaryArrear where monthlysalary_id is null and empid =" + empid;
                var session = NHibernateHelper.OpenSession();
                var result = session.CreateSQLQuery(query).List();
                IList<ISalaryArrear> obj = new List<ISalaryArrear>();
                foreach (object[] dr in result)
                {
                    ISalaryArrear bl = new SalaryArrear();
                    bl.Id = Convert.ToInt32(dr[0]);
                    bl.MonthYear = dr[1].ToString();
                    bl.Basic = Convert.ToDouble(dr[2]);
                    bl.HRA = Convert.ToDouble(dr[3]);
                    bl.Conveyance = Convert.ToDouble(dr[4]);
                    bl.Medical = Convert.ToDouble(dr[5]);
                    bl.PF = Convert.ToDouble(dr[6]);
                    bl.ESI = Convert.ToDouble(dr[7]);
                    bl.EmpId = Convert.ToInt32(dr[9]);
                    obj.Add(bl);
                }
                return obj;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IList<IMonthlySalary> GetEmpMonth(int empid, string month)
        {
            IList<IMonthlySalary> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IMonthlySalary))
               .CreateAlias("Employee", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("Employee.Id", empid))
               .Add(NHibernate.Criterion.Restrictions.Eq("MonthYear", month))
               .List<IMonthlySalary>();
            return obj;
        }
    }

    public class SalaryArrearDAL : Common.CommonDAL<ISalaryArrear>
    {

    }
}