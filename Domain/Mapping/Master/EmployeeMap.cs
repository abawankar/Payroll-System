using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.Master;

namespace WebPayroll.Domain.Mapping
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References<Company>(x => x.Company).Column("CompId").ForeignKey("fk_emp_compid").LazyLoad();
            References<Branch>(x => x.Branch).Column("BranchId").ForeignKey("fk_emp_branchid").LazyLoad();
            References<Department>(x => x.Department).Column("DeptId").ForeignKey("fk_emp_deptid").LazyLoad();
            References<Designation>(x => x.Designation).Column("DesgId").ForeignKey("fk_emp_desgid").LazyLoad();
            Map(x => x.EmpCode);
            Map(x => x.Name);
            Map(x => x.Gender);
            Map(x => x.MarritalStatus).CustomSqlType("nvarchar(1)");
            Map(x => x.DOB);
            Map(x => x.FatherOrHusbandName);
            Map(x => x.FNHFlag).CustomSqlType("nvarchar(1)");
            Map(x => x.UAN);
            Map(x => x.ESIIP);
            Map(x => x.DOJ);
            Map(x => x.DOE);
            Map(x => x.Status);
            Map(x => x.TranType);
            References<StatutoryCode>(x => x.StatutoryCode).Column("StatusId").ForeignKey("fk_emp_statuid");
            HasMany<EmployeeKYC>(x => x.KYC).Cascade.All().LazyLoad();
            HasMany<EmployeePastWorking>(x => x.PastWorkings).Cascade.All().LazyLoad();
        }
    }
}