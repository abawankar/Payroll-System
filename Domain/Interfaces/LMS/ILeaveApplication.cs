using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces.LMS
{
    public interface ILeaveApplication
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        IEmployee Employee { get; set; }
        DateTime DateFrom { get; set; }
        int PeriodFrom { get; set; }
        DateTime DateTo { get; set; }
        int PeriodTo { get; set; }
        ILeaveType LeaveType { get; set; }
        string LeaveReason { get; set; }
        int Status { get; set; }
        double TotalLeave { get; set; }
    }
}
