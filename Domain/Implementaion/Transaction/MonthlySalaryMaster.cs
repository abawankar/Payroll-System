using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class MonthlySalaryMaster : IMonthlySalaryMaster
    {
        public virtual int Id { get; set; }
        public virtual IBranch Branch { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual string Cheque { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual IList<IMonthlySalary> MonthlySalary { get; set; }

        public MonthlySalaryMaster()
        {
            MonthlySalary = new List<IMonthlySalary>();
        }
    }
}