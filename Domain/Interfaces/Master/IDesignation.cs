using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPayroll.Domain.Interfaces
{
    public interface IDesignation
    {
        int Id { get; set; }
        string Name { get; set; }
        double PaidLeave { get; set; }
    }
}