using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.LMS;

namespace WebPayroll.Domain.Mapping.LMS
{
    public class MonthlyAttendanceMap : ClassMap<MonthlyAttendance>
    {
        public MonthlyAttendanceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.Days);
            Map(x => x.Month);
            Map(x => x.Year);
            Map(x => x.LeaveType);
            References<Employee>(x => x.Employee).Column("EmpId");
        }
    }
}