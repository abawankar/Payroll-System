using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Transaction
{
    public class LeaveOBDAL : Common.CommonDAL<ILeaveOB>
    {
        public virtual IList<ILeaveOB> IsLeaveAdded(int empid,int leave,int year)
        {
            IList<ILeaveOB> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(ILeaveOB))
               .Add(NHibernate.Criterion.Restrictions.Eq("Employee.Id", empid))
               .Add(NHibernate.Criterion.Restrictions.Eq("LeaveType.Id", leave))
               .Add(NHibernate.Criterion.Restrictions.Eq("Year", year))
               .List<ILeaveOB>();
            return obj;
        }
    }


}