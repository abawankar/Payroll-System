using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class AppsUser : IAppsUser
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string AppLogin { get; set; }
        public virtual string Role { get; set; }
        public virtual IList<IUserRight> UserRight { get; set; }
        public virtual int Status { get; set; }
    }
}