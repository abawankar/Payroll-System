using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface ILeaveType
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
