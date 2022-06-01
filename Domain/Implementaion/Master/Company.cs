using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class Company : ICompany
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string PAN { get; set; }
        public virtual string GST { get; set; }
        public virtual string Address { get; set; }
        public virtual string CIN { get; set; }
        public virtual string EstablishmentCode { get; set; }
        public virtual string ESIRegistrationNumber { get; set; }
    }
}