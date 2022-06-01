using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class MonthlySalaryMasterMap:ClassMap<MonthlySalaryMaster>
    {
        public MonthlySalaryMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.MonthYear);
            Map(x => x.Cheque);
            Map(x => x.Date);

            References<Branch>(x => x.Branch).Column("BranchId").ForeignKey("fk_salmas_branchid").LazyLoad();
            HasMany<MonthlySalary>(x => x.MonthlySalary).Cascade.All();

        }
    }
}