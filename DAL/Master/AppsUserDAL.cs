using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Common;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Master
{
    public class AppsUserDAL: Common.CommonDAL<IAppsUser>
    {
        public static IAppsUser GetByAppLogin(string Emailid)
        {
            IAppsUser obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IAppsUser))
            .Add(NHibernate.Criterion.Restrictions.Eq("AppLogin", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IAppsUser>();

            return obj;
        }

        public static string GetName(string Emailid)
        {
            IAppsUser obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IAppsUser))
            .Add(NHibernate.Criterion.Restrictions.Eq("AppLogin", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IAppsUser>();

            return obj.Name;
        }

        public void AddRights(int empId, string rightsList)
        {
            IAppsUser bl = GetById(empId);
            UserRightsDAL dal = new UserRightsDAL();
            string[] rightsid = rightsList.Split(',');
            List<IUserRight> list = new List<IUserRight>();
            for (int i = 1; i < rightsid.Length; i++)
            {
                int id = Convert.ToInt32(rightsid[i]);
                IUserRight empRights = dal.GetById(id);
                bl.UserRight.Add(empRights);
            }
            InsertOrUpdate(bl);
        }

        public static bool IsUserExist(string Emailid)
        {
            IAppsUser obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(IAppsUser))
            .Add(NHibernate.Criterion.Restrictions.Eq("AppLogin", Emailid))
            .SetFirstResult(0)
            .UniqueResult<IAppsUser>();

            if (obj == null)
                return false;
            else
                return true;
        }
    }
}