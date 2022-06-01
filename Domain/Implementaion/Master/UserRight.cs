using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class UserRight : IUserRight
    {
        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string MnuName { get; set; }
        public virtual string TableName { get; set; }
        public virtual string Operation { get; set; }
        public virtual string Description { get; set; }
    }
}