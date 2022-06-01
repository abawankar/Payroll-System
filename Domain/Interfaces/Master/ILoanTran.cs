using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPayroll.Domain.Interfaces
{
    public interface ILoanCRTran
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        double Amount { get; set; }
        string Comments { get; set; }
    }

    public interface ILoanDRTran
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        double Amount { get; set; }
        string PaidBy { get; set; }
        string PaidMonth { get; set; }
        string Comments { get; set; }
    }
}