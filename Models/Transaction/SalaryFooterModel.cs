using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPayroll.Models.Transaction
{
    public class SalaryFooterModel
    {
        public string BranCode { get; set; }
        public int BranchId { get; set; }
        public string MonthYear { get; set; }
        public int NoOfEmp { get; set; }
        public double Basic { get; set; }
        public double DA { get; set; }
        public double HRA { get; set; }
        public double Conveyance { get; set; }
        public double Medical { get; set; }
        public double EduAllowance { get; set; }
        public double TelephoneReimb { get; set; }
        public double SatutoryBonus { get; set; }
        public double CarRunningReimb { get; set; }
        public double OtherAllowance { get; set; }
        public double PF { get; set; }
        public double VPF { get; set; }
        public double ESI { get; set; }
        public double TDS { get; set; }
        public double LoanAmount { get; set; }
        public double GrossSalary
        {
            get { return Basic + DA + HRA + Conveyance + Medical + EduAllowance + TelephoneReimb + CarRunningReimb + SatutoryBonus + OtherAllowance; }
        }
        public double NetDedn
        {
            get
            {
                return PF + ESI + VPF + LoanAmount+TDS;
            }
        }
        public double NetSalary
        {
            get { return GrossSalary - NetDedn; }
        }
    }
}