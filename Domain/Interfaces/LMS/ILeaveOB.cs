using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface ILeaveOB
    {
        int Id { get; set; }
        int Year { get; set; }
        IEmployee Employee { get; set; }
        ILeaveType LeaveType { get; set; }
        double BalanceLeave { get; set; }
    }
}
