using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Interfaces.LMS;

namespace WebPayroll.DAL.LMS
{
    public class MonthlyAttendanceDAL : Common.CommonDAL<IMonthlyAttendance>
    {
        public IList<IMonthlyAttendance> GetByEmployee(int empid)
        {
            IList<IMonthlyAttendance> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IMonthlyAttendance))
               .Add(NHibernate.Criterion.Restrictions.Eq("Employee.Id", empid))
               .List<IMonthlyAttendance>();
            return obj;
        }
    }
}