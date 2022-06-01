using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces.LMS
{
    public interface ILeaveDetails
    {
        int Id { get; set; }
        int Year { get; set; }
        IEmployee Employee { get; set; }
        double PaidLeave { get; set; }
        double Extra { get; set; }
        double UnPaid { get; set; }
        double JAN { get; set; }
        double FEB { get; set; }
        double MAR { get; set; }
        double APR { get; set; }
        double MAY { get; set; }
        double JUN { get; set; }
        double JUL { get; set; }
        double AUG { get; set; }
        double SEP { get; set; }
        double OCT { get; set; }
        double NOV { get; set; }
        double DEC { get; set; }
        IList<ILeaveDeduction> Deduction { get; set; }

    }
}
