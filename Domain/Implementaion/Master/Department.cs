using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class Department : IDepartment
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
    }
}