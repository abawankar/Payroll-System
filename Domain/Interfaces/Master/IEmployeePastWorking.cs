using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces.Master
{
    public interface IEmployeePastWorking
    {
        int Id { get; set; }
        string CompanyName { get; set; }
        DateTime DateFrom { get; set; }
        DateTime DateTo { get; set; }
        int Rejoin { get; set; }
        string Comments { get; set; }

    }
}
