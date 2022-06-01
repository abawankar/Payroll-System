using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Domain.Interfaces.Master;

namespace WebPayroll.Domain.Implementaion
{
    public class Employee : IEmployee
    {
        public virtual int Id { get; set; }
        public virtual ICompany Company { get; set; }
        public virtual IBranch Branch { get; set; }
        public virtual IDepartment Department { get; set; }
        public virtual IDesignation Designation { get; set; }
        public virtual string EmpCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string Gender { get; set; }
        public virtual string MarritalStatus { get; set; }
        public virtual DateTime? DOB { get; set; }
        public virtual string FatherOrHusbandName { get; set; }
        public virtual string FNHFlag { get; set; }
        public virtual string UAN { get; set; }
        public virtual string ESIIP { get; set; }
        public virtual DateTime? DOJ { get; set; }
        public virtual DateTime? DOE { get; set; }
        public virtual int Status { get; set; }
        public virtual int TranType { get; set; }
        public virtual IStatutoryCode StatutoryCode { get; set; }
        public virtual IList<IEmployeeKYC> KYC { get; set; }
        public virtual IList<IEmployeePastWorking> PastWorkings { get; set; }
        public Employee()
        {
            KYC = new List<IEmployeeKYC>();
            PastWorkings = new List<IEmployeePastWorking>();
        }

    }
}