using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IBranch
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
    }
}
