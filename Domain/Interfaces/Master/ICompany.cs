using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebPayroll.Domain.Interfaces
{
    public interface ICompany
    {
        int Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string PAN { get; set; }
        string GST { get; set; }
        string Address { get; set; }
        string CIN { get; set; }
        string EstablishmentCode { get; set; }
        string ESIRegistrationNumber { get; set; }

    }
}
