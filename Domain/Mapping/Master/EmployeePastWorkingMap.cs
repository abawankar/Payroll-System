using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion.Master;

namespace WebPayroll.Domain.Mapping.Master
{
    public class EmployeePastWorkingMap : ClassMap<EmployeePastWorking>
    {
        public EmployeePastWorkingMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.CompanyName);
            Map(x => x.DateFrom);
            Map(x => x.DateTo);
            Map(x => x.Rejoin);
            Map(x => x.Comments);
        }
    }
}