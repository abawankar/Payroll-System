using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Domain.Interfaces.LMS;

namespace WebPayroll.Domain.Implementaion.LMS
{
    public class LeaveDetails : ILeaveDetails
    {
        public virtual int Id { get; set; }
        public virtual int Year { get; set; }
        public virtual IEmployee Employee { get; set; }
        public virtual double PaidLeave { get; set; }
        public virtual double Extra { get; set; }
        public virtual double UnPaid { get; set; }
        public virtual double JAN { get; set; }
        public virtual double FEB { get; set; }
        public virtual double MAR { get; set; }
        public virtual double APR { get; set; }
        public virtual double MAY { get; set; }
        public virtual double JUN { get; set; }
        public virtual double JUL { get; set; }
        public virtual double AUG { get; set; }
        public virtual double SEP { get; set; }
        public virtual double OCT { get; set; }
        public virtual double NOV { get; set; }
        public virtual double DEC { get; set; }
        public virtual IList<ILeaveDeduction> Deduction { get; set; }

        public LeaveDetails()
        {
            Deduction = new List<ILeaveDeduction>();
        }

    }
}