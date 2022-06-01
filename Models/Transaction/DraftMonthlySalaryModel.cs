using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Transaction
{
    public class DraftMonthlySalaryModel:Domain.Implementaion.DraftMonthlySalary
    {
        public int EmpId { get; set; }
        public int BranId { get; set; }
        public int LoanId { get; set; }
        public double GrossSalary
        {
            get { return Basic + DA + HRA + Conveyance + Medical + EduAllowance + TelephoneReimb + CarRunningReimb + SatutoryBonus + OtherAllowance; }
        }
        public double LoanAmount
        {
            get { return Loan != null ? Loan.Amount : 0; }
        }
        public double NetDedn { get {
                return PF + ESI + VPF + LoanAmount+TDS;
            } }
        public double NetSalary
        {
            get { return GrossSalary - NetDedn; }
        }
    }

    public class DraftMonthlySalaryRepository : RepositoryModel<DraftMonthlySalaryModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(DraftMonthlySalaryModel obj)
        {
            DraftMonthlySalaryDAL dal = new DraftMonthlySalaryDAL();
            IDraftMonthlySalary bl = dal.GetById(obj.Id);
            bl.Basic = obj.Basic;
            bl.DA = obj.DA;
            bl.HRA = obj.HRA;
            bl.Conveyance = obj.Conveyance;
            bl.Medical = obj.Medical;
            bl.EduAllowance = obj.EduAllowance;
            bl.TelephoneReimb = obj.TelephoneReimb;
            bl.CarRunningReimb = obj.CarRunningReimb;
            bl.SatutoryBonus = obj.SatutoryBonus;
            bl.OtherAllowance = obj.OtherAllowance;
            bl.PaidDays = obj.PaidDays;
            bl.PF = obj.PF;
            bl.ESI = obj.ESI;
            bl.TDS = obj.TDS;
            dal.InsertOrUpdate(bl);
        }

        public override IList<DraftMonthlySalaryModel> GetAll()
        {
            DraftMonthlySalaryDAL dal = new DraftMonthlySalaryDAL();
            AutoMapper.Mapper.CreateMap<DraftMonthlySalary, DraftMonthlySalaryModel>();
            AutoMapper.Mapper.CreateMap<DraftMonthlySalary, DraftMonthlySalaryModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranId, opt => opt.MapFrom(scr => scr.Branch.Id))
                .ForMember(dest => dest.LoanId, opt => opt.MapFrom(scr => scr.Loan.Id));
            List<DraftMonthlySalaryModel> model = AutoMapper.Mapper.Map<List<DraftMonthlySalaryModel>>(dal.GetAll());

            return model;
        }

        public override DraftMonthlySalaryModel GetById(int id)
        {
            DraftMonthlySalaryDAL dal = new DraftMonthlySalaryDAL();
            AutoMapper.Mapper.CreateMap<DraftMonthlySalary, DraftMonthlySalaryModel>();
            AutoMapper.Mapper.CreateMap<DraftMonthlySalary, DraftMonthlySalaryModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranId, opt => opt.MapFrom(scr => scr.Branch.Id))
                .ForMember(dest => dest.LoanId, opt => opt.MapFrom(scr => scr.Loan.Id));
            DraftMonthlySalaryModel model = AutoMapper.Mapper.Map<DraftMonthlySalaryModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(DraftMonthlySalaryModel obj)
        {
            throw new NotImplementedException();
        }
    }
}