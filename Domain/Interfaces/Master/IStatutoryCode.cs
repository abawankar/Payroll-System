using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IStatutoryCode
    {
        int Id { get; set; }
        string Code { get; set; }
        double PFCont { get; set; }
        double ESICont { get; set; }
        double PFCelling { get; set; }
        double ESICelling { get; set; }
    }
}
