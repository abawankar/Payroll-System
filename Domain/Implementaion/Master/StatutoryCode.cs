using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class StatutoryCode : IStatutoryCode
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual double PFCont { get; set; }
        public virtual double ESICont { get; set; }
        public virtual double PFCelling { get; set; }
        public virtual double ESICelling { get; set; }
    }
}