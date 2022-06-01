using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class Designation : IDesignation
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double PaidLeave { get; set; }
    }
}