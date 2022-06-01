using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces.LMS
{
    public interface IMonthlyAttendance
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        int Days { get; set; }
        int Month { get; set; }
        int Year { get; set; }
        string LeaveType { get; set; }
        IEmployee Employee { get; set; }
    }

}
