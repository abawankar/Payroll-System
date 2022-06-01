using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class SalaryMaster : ISalaryMaster
    {
        public virtual int Id { get; set; }
        public virtual IEmployee Employee { get; set; }
        public virtual double Basic { get; set; }
        public virtual double DA { get; set; }
        public virtual double HRA { get; set; }
        public virtual double Conveyance { get; set; }
        public virtual double Medical { get; set; }
        public virtual double EduAllowance { get; set; }
        public virtual double TelephoneReimb { get; set; }
        public virtual double SatutoryBonus { get; set; }
        public virtual double CarRunningReimb { get; set; }
        public virtual double OtherAllowance { get; set; }
        public virtual double PF { get; set; }
        public virtual double VPF { get; set; }
        public virtual double ESI { get; set; }
        public virtual double TDS { get; set; }
        public virtual string IsPFExempted { get; set; }
        public virtual string IsESIExempted { get; set; }
        public virtual string IsPFCelling { get; set; }
    }
}