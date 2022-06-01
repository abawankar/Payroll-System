using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Master
{
    public class EmployeeDAL : Common.CommonDAL<IEmployee>
    {
        public int Insert(IEmployee obj)
        {
            ISession session = NHibernateHelper.OpenSession();
            using (ITransaction transcation = session.BeginTransaction())
            {
                try
                {
                    session.Flush();
                    session.SaveOrUpdate(obj);
                    transcation.Commit();
                    return obj.Id;
                }
                catch (Exception ex)
                {
                    transcation.Rollback();
                    throw ex;
                }
            }
        }

        public IList<IEmployee> GetActiveByBranch(int branchid)
        {
            IList<IEmployee> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IEmployee))
               .Add(NHibernate.Criterion.Restrictions.Eq("Branch.Id", branchid))
               .Add(NHibernate.Criterion.Restrictions.Eq("Status", 1))
               .List<IEmployee>();
            return obj;
        }

        public IList<IEmployee> GetByBranch(int branchid)
        {
            IList<IEmployee> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(IEmployee))
               .Add(NHibernate.Criterion.Restrictions.Eq("Branch.Id", branchid))
               .List<IEmployee>();
            return obj;
        }
    }
}