using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.LMS;

namespace WebPayroll.Domain.Mapping.LMS
{
    public class LeaveDetailsMap : ClassMap<LeaveDetails>
    {
        public LeaveDetailsMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Year);
            Map(x => x.PaidLeave).CustomSqlType("numeric(12,2)");
            Map(x => x.Extra).CustomSqlType("numeric(12,2)");
            Map(x => x.UnPaid).CustomSqlType("numeric(12,2)");
            Map(x => x.JAN).CustomSqlType("numeric(12,2)");
            Map(x => x.FEB).CustomSqlType("numeric(12,2)");
            Map(x => x.MAR).CustomSqlType("numeric(12,2)");
            Map(x => x.APR).CustomSqlType("numeric(12,2)");
            Map(x => x.MAY).CustomSqlType("numeric(12,2)");
            Map(x => x.JUN).CustomSqlType("numeric(12,2)");
            Map(x => x.JUL).CustomSqlType("numeric(12,2)");
            Map(x => x.AUG).CustomSqlType("numeric(12,2)");
            Map(x => x.SEP).CustomSqlType("numeric(12,2)");
            Map(x => x.OCT).CustomSqlType("numeric(12,2)");
            Map(x => x.NOV).CustomSqlType("numeric(12,2)");
            Map(x => x.DEC).CustomSqlType("numeric(12,2)");
            References<Employee>(x => x.Employee).Column("EmpId").ForeignKey("fk_leavedetails_empid").LazyLoad();
            HasMany<LeaveDeduction>(x => x.Deduction).KeyColumn("LeaveId").Cascade.All();
        }
    }
}