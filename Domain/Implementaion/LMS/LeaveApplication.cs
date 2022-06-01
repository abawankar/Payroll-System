using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Domain.Interfaces.LMS;

namespace WebPayroll.Domain.Implementaion.LMS
{
    public class LeaveApplication : ILeaveApplication
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual IEmployee Employee { get; set; }
        public virtual DateTime DateFrom { get; set; }
        public virtual int PeriodFrom { get; set; }
        public virtual DateTime DateTo { get; set; }
        public virtual int PeriodTo { get; set; }
        public virtual ILeaveType LeaveType { get; set; }
        public virtual string LeaveReason { get; set; }
        public virtual int Status { get; set; }
        public virtual double TotalLeave { get; set; }
    }
}