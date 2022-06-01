using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IUserRight
    {
        int Id { get; set; }
        string Code { get; set; }
        string MnuName { get; set; }
        string TableName { get; set; }
        string Operation { get; set; }
        string Description { get; set; }
    }
}
