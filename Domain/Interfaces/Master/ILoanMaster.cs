using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface ILoanMaster 
    {
        int Id { get; set; }
        IEmployee Employee { get; set; }
        string LoanCode { get; set; }
        string Type { get; set; }
        DateTime Date { get; set; } 
        string PaymentMode { get; set; }
        DateTime DednFrom { get; set; }
        double DednAmount { get; set; }
        string Comments { get; set; }
        int Status { get; set; }
        IList<ILoanCRTran> LoanCRTran { get; set; }
        IList<ILoanDRTran> LoanDRTran { get; set; }

    }
}
