using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class LoanCRTranMap : ClassMap<LoanCRTran>
    {
        public LoanCRTranMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.Amount);
            Map(x => x.Comments);
        }
    }

    public class LoanDRTranMap : ClassMap<LoanDRTran>
    {
        public LoanDRTranMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.Amount);
            Map(x => x.PaidBy).CustomSqlType("nvarchar(1)");
            Map(x => x.PaidMonth).CustomSqlType("nvarchar(7)");
            Map(x => x.Comments);
        }
    }
}