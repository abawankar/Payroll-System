using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Domain.Mapping
{
    public class StatutoryCodeMap:ClassMap<StatutoryCode>
    {
        public StatutoryCodeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code);
            Map(x => x.PFCont);
            Map(x => x.ESICont);
            Map(x => x.PFCelling);
            Map(x => x.ESICelling);

        }
    }
}