using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPayroll.Domain.Interfaces.Master;

namespace WebPayroll.Domain.Interfaces
{
    public interface IEmployee
    {
        int Id { get; set; }
        ICompany Company { get; set; }
        IBranch Branch { get; set; }
        IDepartment Department { get; set; }
        IDesignation Designation { get; set; }
        string EmpCode { get; set; }
        string Name { get; set; }
        string Gender { get; set; }
        string MarritalStatus { get; set; }
        DateTime? DOB { get; set; }
        string FatherOrHusbandName { get; set; }
        string FNHFlag { get; set; }
        string UAN { get; set; }
        string ESIIP { get; set; }
        DateTime? DOJ { get; set; }
        DateTime? DOE { get; set; }
        int Status { get; set; }
        IStatutoryCode StatutoryCode { get; set; }
        IList<IEmployeeKYC> KYC { get; set; }
        IList<IEmployeePastWorking> PastWorkings { get; set; }
        int TranType { get; set; }
    }
}
