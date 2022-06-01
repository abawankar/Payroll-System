using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IDraftMonthlySalary
    {
        int Id { get; set; }
        IBranch Branch { get; set; }
        IEmployee Employee { get; set; }
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
        string MontYear { get; set; }
        ILoanDRTran Loan { get; set; }
    }
}
