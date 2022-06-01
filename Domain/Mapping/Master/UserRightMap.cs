using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class UserRightMap : ClassMap<UserRight>
    {
        public UserRightMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.MnuName);
            Map(x => x.TableName);
            Map(x => x.Operation);
            Map(x => x.Description);
        }
    }
}