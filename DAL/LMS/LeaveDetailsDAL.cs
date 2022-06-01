using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Interfaces.LMS;

namespace WebPayroll.DAL.LMS
{
    public class LeaveDetailsDAL : Common.CommonDAL<ILeaveDetails>
    {
        public IList<ILeaveDetails> GetActiveByBranch(int branchid,int yr)
        {
            if (yr == 0) yr = System.DateTime.Today.Year;

            IList<ILeaveDetails> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ILeaveDetails))
               .CreateAlias("Employee", "emp")
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Branch.Id", branchid))
               .Add(NHibernate.Criterion.Restrictions.Eq("emp.Status", 1))
               .Add(NHibernate.Criterion.Restrictions.Eq("Year", yr))
               .List<ILeaveDetails>();
            return obj;
        }
    }
}