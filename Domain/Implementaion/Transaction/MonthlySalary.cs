using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Domain.Implementaion
{
    public class MonthlySalary : IMonthlySalary
    {
        public virtual int Id { get; set; }
        public virtual IEmployee Employee { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual double Basic { get; set; }
        public virtual double DA { get; set; }
        public virtual double HRA { get; set; }
        public virtual double Conveyance { get; set; }
        public virtual double Medical { get; set; }
        public virtual double EduAllowance { get; set; }
        public virtual double TelephoneReimb { get; set; }
        public virtual double SatutoryBonus { get; set; }
        public virtual double CarRunningReimb { get; set; }
        public virtual double OtherAllowance { get; set; }
        public virtual double PF { get; set; }
        public virtual double VPF { get; set; }
        public virtual double ESI { get; set; }
        public virtual double TDS { get; set; }
        public virtual int PaidDays { get; set; }
        public virtual double LoanAmount { get; set; }
        public virtual IList<ISalaryArrear> SalaryArrear { get; set; }
        public virtual double Arrear
        {
            get
            {

                if (SalaryArrear != null)
                {
                    return SalaryArrear.Sum(x => x.NetSalary);
                }
                else return 0;

            }
        }
        public virtual double GrossSalary
        {
            get { return Basic + DA + HRA + Conveyance + Medical + EduAllowance + TelephoneReimb + CarRunningReimb+SatutoryBonus+OtherAllowance; }
        }
        public virtual double NetDedn
        {
            get
            {
                return PF + ESI + VPF + LoanAmount + TDS;
            }
        }
        public virtual double NetSalary
        {
            get { return GrossSalary + Arrear - NetDedn; }
        }
        
    }

    public class SalaryArrear : ISalaryArrear
    {
        public virtual int Id { get; set; }
        public virtual int EmpId { get; set; }
        public virtual string MonthYear { get; set; }
        public virtual double Basic { get; set; }
        public virtual double HRA { get; set; }
        public virtual double Conveyance { get; set; }
        public virtual double Medical { get; set; }
        public virtual double EduAllowance { get; set; }
        public virtual double TelephoneReimb { get; set; }
        public virtual double SatutoryBonus { get; set; }
        public virtual double CarRunningReimb { get; set; }
        public virtual double OtherAllowance { get; set; }
        public virtual double PF { get; set; }
        public virtual double ESI { get; set; }
        public virtual double GrossSalary
        {
            get { return Basic + HRA + Conveyance + Medical + EduAllowance + TelephoneReimb + CarRunningReimb + SatutoryBonus + OtherAllowance; }
        }
        public virtual double NetSalary
        {
            get { return GrossSalary - PF-ESI; }
        }
    }
}