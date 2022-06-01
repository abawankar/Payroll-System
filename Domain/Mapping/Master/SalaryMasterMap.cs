using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class SalaryMasterMap : ClassMap<SalaryMaster>
    {
        public SalaryMasterMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
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
            Map(x => x.IsPFExempted);
            Map(x => x.IsESIExempted);
            Map(x => x.IsPFCelling);

            References<Employee>(x => x.Employee).Column("EmpId").ForeignKey("fk_salarymaster_empid").LazyLoad();
        }
    }
}