using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces.Master;

namespace WebPayroll.Domain.Implementaion.Master
{
    public class EmployeePastWorking : IEmployeePastWorking
    {
        public virtual int Id { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual DateTime DateFrom { get; set; }
        public virtual DateTime DateTo { get; set; }
        public virtual int Rejoin { get; set; }
        public virtual string Comments { get; set; }
    }
}