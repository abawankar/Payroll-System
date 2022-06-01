using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPayroll.Models.Report
{
    public class MonthlyDednReportModel
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public double LoanTaken { get; set; }
        public double LoanPaid { get; set; }
        public double Balance { get; set; }
        public double ThisMonth { get; set; }
    }
}