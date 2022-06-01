using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class LeaveOBMap : ClassMap<LeaveOB>
    {
        public LeaveOBMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Year);
            Map(x => x.BalanceLeave).CustomSqlType("numeric(12,2)").Not.Nullable();
            References<Employee>(x => x.Employee).Column("EmpId").ForeignKey("fk_leaveob_empid").LazyLoad();
            References<LeaveType>(x => x.LeaveType).Column("LeaveTId").ForeignKey("fk_leaveob_LeaveTId").LazyLoad();
        }
    }
}