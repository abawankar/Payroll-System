using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class CompanyMap : ClassMap<Company>
    {
        public CompanyMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.Name);
            Map(x => x.PAN);
            Map(x => x.GST);
            Map(x => x.Address);
            Map(x => x.CIN);
            Map(x => x.EstablishmentCode);
            Map(x => x.ESIRegistrationNumber);
        }
    }
}