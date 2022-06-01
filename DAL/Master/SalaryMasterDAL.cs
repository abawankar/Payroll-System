using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Master
{
    public class SalaryMasterDAL: Common.CommonDAL<ISalaryMaster>
    {
        public IList<ISalaryMaster> GetActive(int status)
        {
            IList<ISalaryMaster> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ISalaryMaster))
               .CreateAlias("Employee", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Status", status))
               .List<ISalaryMaster>();
            return obj;
        }

        public IList<ISalaryMaster> GetByCompBran(int compid, int branid)
        {
            IList<ISalaryMaster> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ISalaryMaster))
               .CreateAlias("Employee", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Company.Id", compid))
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Branch.Id", branid))
               .List<ISalaryMaster>();
            return obj;
        }

        public IList<ISalaryMaster> GetByBranch(int branid)
        {
            IList<ISalaryMaster> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ISalaryMaster))
               .CreateAlias("Employee", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Branch.Id", branid))
               .List<ISalaryMaster>();
            return obj;
        }

    }
}