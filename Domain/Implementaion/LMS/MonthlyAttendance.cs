using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Domain.Interfaces.LMS;

namespace WebPayroll.Domain.Implementaion.LMS
{
    public class MonthlyAttendance : IMonthlyAttendance
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Days { get; set; }
        public virtual int Month { get; set; }
        public virtual int Year { get; set; }
        public virtual string LeaveType { get; set; }
        public virtual IEmployee Employee { get; set; }
    }

    
}