using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface IAppsUser
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string AppLogin { get; set; }
        string Role { get; set; }
        IList<IUserRight> UserRight { get; set; }
        int Status { get; set; }
    }
}
