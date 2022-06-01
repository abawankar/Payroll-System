using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IMonthlySalary
    {
        int Id { get; set; }
        IEmployee Employee { get; set; }
        string MonthYear { get; set; }
        double Basic { get; set; }
        double DA { get; set; }
        double HRA { get; set; }
        double Conveyance { get; set; }
        double Medical { get; set; }
        double EduAllowance { get; set; }
        double TelephoneReimb { get; set; }
        double SatutoryBonus { get; set; }
        double CarRunningReimb { get; set; }
        double OtherAllowance { get; set; }
        double PF { get; set; }
        double VPF { get; set; }
        double ESI { get; set; }
        double TDS { get; set; }
        int PaidDays { get; set; }
        double LoanAmount { get; set; }
        double GrossSalary { get;}
        double NetDedn { get; }
        double NetSalary { get; }
        double Arrear { get; }
        IList<ISalaryArrear> SalaryArrear { get; set; }
    }

    public interface ISalaryArrear
    {
        int Id { get; set; }
        int EmpId { get; set; }
        string MonthYear { get; set; }
        double Basic { get; set; }
        double  HRA { get; set; }
        double Conveyance { get; set; }
        double Medical { get; set; }
        double EduAllowance { get; set; }
        double TelephoneReimb { get; set; }
        double SatutoryBonus { get; set; }
        double CarRunningReimb { get; set; }
        double OtherAllowance { get; set; }
        double PF { get; set; }
        double ESI { get; set; }
        double GrossSalary { get;}
        double NetSalary { get; }
    }
}
