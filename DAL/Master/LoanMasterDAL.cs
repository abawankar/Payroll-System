using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Master
{
    public class LoanMasterDAL : Common.CommonDAL<ILoanMaster>
    {

        public ILoanMaster GetByEmpId(int empid)
        {
            ILoanMaster obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(ILoanMaster))
            .Add(NHibernate.Criterion.Restrictions.Eq("Employee.Id", empid))
            .SetFirstResult(0)
            .UniqueResult<ILoanMaster>();

            return obj;
        }
    }
}