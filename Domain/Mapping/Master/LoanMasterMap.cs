using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class LoanMasterMap : ClassMap<LoanMaster>
    {
        public LoanMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x=>x.LoanCode);
            Map(x=>x.Type);
            Map(x=>x.Date);
            Map(x=>x.PaymentMode);
            Map(x=>x.DednFrom);
            Map(x => x.DednAmount).CustomSqlType("numeric(12,2)");
            Map(x=>x.Comments);
            Map(x => x.Status);
            References<Employee>(x => x.Employee).Column("EmpId").ForeignKey("fk_loan_empid").LazyLoad();
            HasMany<LoanCRTran>(x => x.LoanCRTran).Cascade.All();
            HasMany<LoanDRTran>(x => x.LoanDRTran).Cascade.All();
        }
    }
}