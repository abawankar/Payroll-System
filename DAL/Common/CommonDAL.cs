using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Common
{
    public abstract class CommonDAL<T>
    {
        public virtual T GetById(int id)
        {
            T obj = NHibernateHelper
            .OpenSession()
            .CreateCriteria(typeof(T))
            .Add(NHibernate.Criterion.Restrictions.Eq("Id", id))
            .SetFirstResult(0)
            .UniqueResult<T>();

            return obj;
        }

        public virtual IList<T> GetAll()
        {
            IList<T> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(T))
               .List<T>();
            return obj;
        }

        public virtual IList<T> GetAll(int maxRecord)
        {
            IList<T> obj = NHibernateHelper.OpenSession()
               .CreateCriteria(typeof(T))
               .AddOrder(Order.Desc("Id"))
               .SetMaxResults(maxRecord)
               .List<T>();
            return obj;
        }

        public void InsertOrUpdate(T obj)
        {
            ISession session = NHibernateHelper.OpenSession();
            using (ITransaction transcation = session.BeginTransaction())
            {
                try
                {
                    session.Flush();
                    session.SaveOrUpdate(obj);
                    transcation.Commit();
                }
                catch (Exception ex)
                {
                    transcation.Rollback();
                    throw ex;
                }
            }
        }

        public void InsertBulk(IList<T> obj)
        {
            ISession session = NHibernateHelper.OpenSession();
            using (ITransaction transcation = session.BeginTransaction())
            {
                try
                {
                    session.Flush();
                    foreach (var item in obj)
                    {
                        session.Save(item);
                    }
                    transcation.Commit();
                }
                catch (Exception ex)
                {
                    transcation.Rollback();
                    throw ex;
                }
            }
        }

        public bool Delete(T obj)
        {
            bool flag;
            ISession session = NHibernateHelper.OpenSession();
            using (ITransaction transcation = session.BeginTransaction())
            {
                try
                {
                    session.Delete(obj);
                    transcation.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    transcation.Rollback();
                    flag = false;
                }

            }
            return flag;
        }

        public bool DeleteBulk(IList<T> obj)
        {
            bool flag;
            ISession session = NHibernateHelper.OpenSession();
            using (ITransaction transcation = session.BeginTransaction())
            {
                try
                {
                    foreach (var item in obj)
                    {
                        session.Delete(item);
                    }

                    transcation.Commit();
                    flag = true;
                }
                catch (Exception)
                {
                    transcation.Rollback();
                    flag = false;
                }

            }
            return flag;
        }
    }

    public static class MyExtension
    {
        public static string ToFinancialYear(this DateTime dateTime)
        {
            return (dateTime.Month >= 4 ? dateTime.ToString("yyyy") + "-" + dateTime.AddYears(1).ToString("yy") : dateTime.ToString("yyyy") + "-" + dateTime.AddYears(1).ToString("yy"));
        }

        public static string FyMonth(this DateTime dateTime)
        {
            string month = dateTime.Month < 10 ? '0' + dateTime.Month.ToString() : dateTime.Month.ToString();

            return month + "/" + dateTime.Year;
        }
    }

    public static class CreateDB
    {
        public static void CreateDatabase(string connectionString)
        {
            try {
                NHibernate.Cfg.Configuration config = Fluently.Configure().Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(c => c.Is(connectionString)))
                     .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(ICompany).Assembly)).CurrentSessionContext<NHibernate.Context.ThreadStaticSessionContext>().BuildConfiguration();

                var schemaExport = new SchemaExport(config);

                schemaExport.Create(false, true);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}