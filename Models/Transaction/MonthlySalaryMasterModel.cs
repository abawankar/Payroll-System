using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Transaction
{
    public class MonthlySalaryMasterModel:Domain.Implementaion.MonthlySalaryMaster
    {
        public int BranchId { get; set; }
        public int NoOfEmp
        {
            get { return MonthlySalary.Count(); }
        }
        public double GrossSalary
        {
            get { return MonthlySalary.Sum(x => x.Basic + x.DA + x.HRA + x.Conveyance + x.Medical + x.EduAllowance + x.TelephoneReimb + x.CarRunningReimb + x.SatutoryBonus + x.OtherAllowance); }
        }
        public double PF
        {
            get { return MonthlySalary.Sum(x => x.PF); }
        }
        public double VPF
        {
            get { return MonthlySalary.Sum(x => x.VPF); }
        }
        public double ESI
        {
            get { return MonthlySalary.Sum(x => x.ESI); }
        }
        public double TDS
        {
            get { return MonthlySalary.Sum(x => x.TDS); }
        }
        public double LoanAmount
        {
            get { return MonthlySalary.Sum(x => x.LoanAmount); }
        }
        public double NetDedn
        {
            get { return PF + VPF + ESI + TDS + LoanAmount; }
        }
        public double Arrear
        {
            get { return MonthlySalary.Sum(x=>x.Arrear); }
        }
        public double NetSalary
        {
            get { return GrossSalary+Arrear - NetDedn; }
        }

    }

    public class MonthlySalaryModel : Domain.Implementaion.MonthlySalary
    {
        public int EmpId { get; set; }
        public int BranchId { get; set; }
        
    }

    public class SalaryArrearModel : Domain.Implementaion.SalaryArrear
    {

    }

    public class MonthlySalaryMasterRepository : RepositoryModel<MonthlySalaryMasterModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(MonthlySalaryMasterModel obj)
        {
            MonthlySalaryMasterDAL dal = new MonthlySalaryMasterDAL();
            IMonthlySalaryMaster bl = dal.GetById(obj.Id);
            bl.Cheque = obj.Cheque;
            bl.Date = obj.Date;
            dal.InsertOrUpdate(bl);
        }

        public override IList<MonthlySalaryMasterModel> GetAll()
        {
            MonthlySalaryMasterDAL dal = new MonthlySalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlySalaryMaster, MonthlySalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlySalaryMaster, MonthlySalaryMasterModel>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Branch.Id));
            List<MonthlySalaryMasterModel> model = AutoMapper.Mapper.Map<List<MonthlySalaryMasterModel>>(dal.GetAll());
            return model;
        }

        public IList<MonthlySalaryModel> GetAll(int id)
        {
            MonthlySalaryMasterDAL dal = new MonthlySalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlySalary, MonthlySalaryModel>();
            AutoMapper.Mapper.CreateMap<MonthlySalary, MonthlySalaryModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<MonthlySalaryModel> model = AutoMapper.Mapper.Map<List<MonthlySalaryModel>>(dal.GetById(id).MonthlySalary);
            return model;
        }

        public override MonthlySalaryMasterModel GetById(int id)
        {
            MonthlySalaryMasterDAL dal = new MonthlySalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<MonthlySalaryMaster, MonthlySalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<MonthlySalaryMaster, MonthlySalaryMasterModel>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Branch.Id));
            MonthlySalaryMasterModel model = AutoMapper.Mapper.Map<MonthlySalaryMasterModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(MonthlySalaryMasterModel obj)
        {
            throw new NotImplementedException();
        }
    }

    public class MonthlySalaryRepository : RepositoryModel<MonthlySalaryModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(MonthlySalaryModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<MonthlySalaryModel> GetAll()
        {
            MonthlySalaryDAL dal = new MonthlySalaryDAL();
            AutoMapper.Mapper.CreateMap<MonthlySalary, MonthlySalaryModel>();
            AutoMapper.Mapper.CreateMap<MonthlySalary, MonthlySalaryModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id));
            List<MonthlySalaryModel> model = AutoMapper.Mapper.Map<List<MonthlySalaryModel>>(dal.GetAll());
            return model;
        }

        public override MonthlySalaryModel GetById(int id)
        {
            MonthlySalaryDAL dal = new MonthlySalaryDAL();
            AutoMapper.Mapper.CreateMap<MonthlySalary, MonthlySalaryModel>();
            AutoMapper.Mapper.CreateMap<MonthlySalary, MonthlySalaryModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id));
            MonthlySalaryModel model = AutoMapper.Mapper.Map<MonthlySalaryModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(MonthlySalaryModel obj)
        {
            throw new NotImplementedException();
        }

        #region --- Salary Arrear ----

        public IList<SalaryArrearModel> GetAllArrear(int id)
        {
            MonthlySalaryDAL dal = new MonthlySalaryDAL();
            AutoMapper.Mapper.CreateMap<SalaryArrear, SalaryArrearModel>();
            List<SalaryArrearModel> model = AutoMapper.Mapper.Map<List<SalaryArrearModel>>(dal.GetById(id).SalaryArrear);
            return model;
        }

        public void InsertArrear(SalaryArrearModel obj, int id)
        {
            MonthlySalaryDAL dal = new MonthlySalaryDAL();
            IMonthlySalary model = dal.GetById(id);

            ISalaryArrear bl = new SalaryArrear();
            bl.MonthYear = obj.MonthYear;
            bl.Basic = obj.Basic;
            bl.HRA = obj.HRA;
            bl.Conveyance = obj.Conveyance;
            bl.Medical = obj.Medical;
            bl.EduAllowance = obj.EduAllowance;
            bl.TelephoneReimb = obj.TelephoneReimb;
            bl.CarRunningReimb = obj.CarRunningReimb;
            bl.SatutoryBonus = obj.SatutoryBonus;
            bl.OtherAllowance = obj.OtherAllowance;
            bl.PF = obj.PF;
            bl.ESI = obj.ESI;

            model.SalaryArrear.Add(bl);
            dal.InsertOrUpdate(model);
        }

        public void EditArrear(SalaryArrearModel obj)
        {
            SalaryArrearDAL dal = new SalaryArrearDAL();
            ISalaryArrear bl = dal.GetById(obj.Id);

            bl.MonthYear = obj.MonthYear;
            bl.Basic = obj.Basic;
            bl.HRA = obj.HRA;
            bl.Conveyance = obj.Conveyance;
            bl.Medical = obj.Medical;
            bl.EduAllowance = obj.EduAllowance;
            bl.TelephoneReimb = obj.TelephoneReimb;
            bl.CarRunningReimb = obj.CarRunningReimb;
            bl.SatutoryBonus = obj.SatutoryBonus;
            bl.OtherAllowance = obj.OtherAllowance;
            bl.PF = obj.PF;
            bl.ESI = obj.ESI;
            dal.InsertOrUpdate(bl);
        }

        public SalaryArrearModel GetByIdArrear(int id)
        {
            SalaryArrearDAL dal = new SalaryArrearDAL();
            AutoMapper.Mapper.CreateMap<SalaryArrear, SalaryArrearModel>();
            SalaryArrearModel model = AutoMapper.Mapper.Map<SalaryArrearModel>(dal.GetById(id));
            return model;
        }

        public bool DeleteArrear(int id)
        {
            SalaryArrearDAL dal = new SalaryArrearDAL();
            ISalaryArrear bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        #endregion
    }

}