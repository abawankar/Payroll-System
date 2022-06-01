using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IMonthlySalaryMaster
    {
        int Id { get; set; }
        IBranch Branch { get; set; }
        string MonthYear { get; set; }
        string Cheque { get; set; }
        DateTime Date { get; set; }
        IList<IMonthlySalary> MonthlySalary { get; set; }
    }
}
