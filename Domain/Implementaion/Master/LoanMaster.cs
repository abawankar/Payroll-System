using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class LoanMaster : ILoanMaster
    {
        public virtual int Id { get; set; }
        public virtual IEmployee Employee { get; set; }
        public virtual string LoanCode { get; set; }
        public virtual string Type { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string PaymentMode { get; set; }
        public virtual DateTime DednFrom { get; set; }
        public virtual double DednAmount { get; set; }
        public virtual string Comments { get; set; }
        public virtual int Status { get; set; }
        public virtual IList<ILoanCRTran> LoanCRTran { get; set; }
        public virtual IList<ILoanDRTran> LoanDRTran { get; set; }

        public LoanMaster()
        {
            LoanCRTran = new List<ILoanCRTran>();
            LoanDRTran = new List<ILoanDRTran>();
        }
    }
}