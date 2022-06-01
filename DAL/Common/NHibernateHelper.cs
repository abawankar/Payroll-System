using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.DAL.Common
{
    public sealed class NHibernateHelper
    {
        public static string SqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        private static NHibernate.ISessionFactory _ISessionFactory;

        private static NHibernate.ISessionFactory SessionFactory
        {
            get
            {
                if (_ISessionFactory == null)
                {
                    _ISessionFactory = Fluently.Configure()
                        .Database(MsSqlConfiguration.MsSql2012.ConnectionString(SqlConnection))

                        .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(ICompany).Assembly))
                        .ExposeConfiguration(c =>
                        {
                            c.Properties.Add("current_session_context_class", "managed_web");
                        })
                        .BuildSessionFactory();
                }

                return _ISessionFactory;
            }
        }

        public static void CheckSession()
        {
            ISession session = NHibernateHelper.OpenSession();
            session.Dispose();
        }

        public static NHibernate.ISession OpenSession()
        {
            if (System.Web.HttpContext.Current != null)
            {
                if (!ManagedWebSessionContext.HasBind(System.Web.HttpContext.Current, _ISessionFactory))
                {
                    ManagedWebSessionContext.Bind(HttpContext.Current, SessionFactory.OpenSession());
                }
            }
            else
            {
                return SessionFactory.OpenSession();
            }

            return SessionFactory.GetCurrentSession();
        }
    }
}