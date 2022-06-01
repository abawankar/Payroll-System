using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.LMS;

namespace WebPayroll.Domain.Mapping.LMS
{
    public class LeaveApplicationMap : ClassMap<LeaveApplication>
    {
        public LeaveApplicationMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(X => X.Date);
            Map(X => X.DateFrom);
            Map(X => X.PeriodFrom);
            Map(X => X.DateTo);
            Map(X => X.PeriodTo);
            Map(X => X.LeaveReason);
            Map(X => X.TotalLeave).CustomSqlType("numeric(12,1)").Not.Nullable();
            Map(x => x.Status);
            References<Employee>(x => x.Employee).Column("EmpId").ForeignKey("fk_lv_empid").LazyLoad();
            References<LeaveType>(x => x.LeaveType).Column("LeaveTypeId").ForeignKey("fk_lv_LeaveTypeId").LazyLoad();
        }
    }
}