using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class LeaveOB : ILeaveOB
    {
        public virtual int Id { get; set; }
        public virtual int Year { get; set; }
        public virtual IEmployee Employee { get; set; }
        public virtual ILeaveType LeaveType { get; set; }
        public virtual double BalanceLeave { get; set; }
    }
}