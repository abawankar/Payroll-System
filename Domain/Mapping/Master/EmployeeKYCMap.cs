using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class EmployeeKYCMap : ClassMap<EmployeeKYC>
    {
        public EmployeeKYCMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DoxType);
            Map(x => x.NameonDox);
            Map(x => x.DocumentNumber);
            Map(x => x.Other);
            Map(x => x.IssueDate);
            Map(x => x.Exipiry);
            Map(x => x.Place);
        }
    }
}