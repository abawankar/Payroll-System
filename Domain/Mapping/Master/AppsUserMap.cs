using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class AppsUserMap : ClassMap<AppsUser>
    {
        public AppsUserMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.Status);
            Map(x => x.AppLogin);
            Map(x => x.Role);
            HasManyToMany<UserRight>(x => x.UserRight).ParentKeyColumn("EmpId").ChildKeyColumn("RightsId").LazyLoad();
        }
    }
}