using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class MonthlySalaryMap:ClassMap<MonthlySalary>
    {
        public MonthlySalaryMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.MonthYear);
            Map(x => x.Basic).CustomSqlType("numeric(12,2)");
            Map(x => x.DA).CustomSqlType("numeric(12,2)");
            Map(x => x.HRA).CustomSqlType("numeric(12,2)");
            Map(x => x.Conveyance).CustomSqlType("numeric(12,2)");
            Map(x => x.Medical).CustomSqlType("numeric(12,2)");
            Map(x => x.EduAllowance).CustomSqlType("numeric(12,2)");
            Map(x => x.TelephoneReimb).CustomSqlType("numeric(12,2)");
            Map(x => x.SatutoryBonus).CustomSqlType("numeric(12,2)");
            Map(x => x.CarRunningReimb).CustomSqlType("numeric(12,2)");
            Map(x => x.OtherAllowance).CustomSqlType("numeric(12,2)");
            Map(x => x.PF).CustomSqlType("numeric(12,2)");
            Map(x => x.VPF).CustomSqlType("numeric(12,2)");
            Map(x => x.ESI).CustomSqlType("numeric(12,2)");
            Map(x => x.TDS).CustomSqlType("numeric(12,2)");
            Map(x => x.PaidDays);
            Map(x => x.LoanAmount).CustomSqlType("numeric(12,2)");
            References<Employee>(x => x.Employee).Column("EmpId").ForeignKey("fk_salary_empid").LazyLoad();

            HasMany<SalaryArrear>(x => x.SalaryArrear).Cascade.All();
        }
    }

    public class SalaryArrearMap: ClassMap<SalaryArrear>
    {
        public SalaryArrearMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.EmpId);
            Map(x => x.MonthYear);
            Map(x => x.Basic).CustomSqlType("numeric(12,2)");
            Map(x => x.HRA).CustomSqlType("numeric(12,2)");
            Map(x => x.Conveyance).CustomSqlType("numeric(12,2)");
            Map(x => x.Medical).CustomSqlType("numeric(12,2)");
            Map(x => x.PF).CustomSqlType("numeric(12,2)");
            Map(x => x.ESI).CustomSqlType("numeric(12,2)");
        }
    }
}