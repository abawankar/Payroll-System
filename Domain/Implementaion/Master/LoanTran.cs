using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class LoanCRTran : ILoanCRTran
    {
       public virtual int Id { get; set; }
       public virtual DateTime Date { get; set; }
       public virtual double Amount { get; set; }
       public virtual string Comments { get; set; }
    }
    public class LoanDRTran : ILoanDRTran
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual double Amount { get; set; }
        public virtual string PaidBy { get; set; }
        public virtual string PaidMonth { get; set; }
        public virtual string Comments { get; set; }
    }
}