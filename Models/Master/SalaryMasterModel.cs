using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class SalaryMasterModel : Domain.Implementaion.SalaryMaster
    {
        public int EmpId { get; set; }
        public string MonthYear { get; set; }
        public string EmpCode { get; set; }
        public int ArMonth { get; set; }
        public double GrossSalary
        {
            get { return Basic + DA + HRA + Conveyance + Medical + EduAllowance + TelephoneReimb + SatutoryBonus + CarRunningReimb+ OtherAllowance; }
        }
        public double NetSalary
        {
            get { return GrossSalary-PF-VPF-ESI-TDS; }
        }
    }

    public class SalaryMasterRepository : RepositoryModel<SalaryMasterModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(SalaryMasterModel obj)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            ISalaryMaster bl = dal.GetById(obj.Id);
            bl.Basic=obj.Basic;
            bl.DA=obj.DA;
            bl.HRA=obj.HRA;
            bl.Conveyance=obj.Conveyance;
            bl.Medical=obj.Medical;
            bl.EduAllowance=obj.EduAllowance;
            bl.TelephoneReimb=obj.TelephoneReimb;
            bl.CarRunningReimb = obj.CarRunningReimb;
            bl.SatutoryBonus = obj.SatutoryBonus;
            bl.OtherAllowance = obj.OtherAllowance;
            bl.Medical = 0;
            bl.Conveyance = 0;
            bl.PF=obj.PF;
            bl.VPF=obj.VPF;
            bl.ESI=obj.ESI;
            bl.TDS = obj.TDS;
            bl.IsPFCelling=obj.IsPFCelling;
            bl.IsPFExempted=obj.IsPFExempted;
            bl.IsESIExempted = obj.IsESIExempted;
            dal.InsertOrUpdate(bl);
        }

        public void SalaryRevision(SalaryMasterModel obj)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            ISalaryMaster bl = dal.GetById(obj.Id);

            SalaryRevisionDAL revdal = new SalaryRevisionDAL();
            ISalaryRevision revBl = new SalaryRevision();

            MonthlySalaryDAL mdal = new MonthlySalaryDAL();
            SalaryArrearDAL aDal = new SalaryArrearDAL();

            //Old Salary
            revBl.MonthYear = obj.MonthYear;
            revBl.Basic = bl.Basic;
            revBl.DA = bl.DA;
            revBl.HRA = bl.HRA;
            revBl.Conveyance = bl.Conveyance;
            revBl.Medical = bl.Medical;
            revBl.EduAllowance = obj.EduAllowance;
            revBl.TelephoneReimb = obj.TelephoneReimb;
            revBl.CarRunningReimb = obj.CarRunningReimb;
            revBl.SatutoryBonus = obj.SatutoryBonus;
            revBl.OtherAllowance = obj.OtherAllowance;
            revBl.PF = bl.PF;
            revBl.VPF = bl.VPF;
            revBl.ESI = bl.ESI;
            revBl.TDS = bl.TDS;
            revBl.IsPFCelling = bl.IsPFCelling;
            revBl.IsPFExempted = bl.IsPFExempted;
            revBl.IsESIExempted = bl.IsESIExempted;
            revBl.Employee = new Employee { Id = obj.EmpId };
            revdal.InsertOrUpdate(revBl);

            //Revised Salary
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
            bl.PF = obj.PF;
            bl.VPF = obj.VPF;
            bl.ESI = obj.ESI;
            bl.TDS = obj.TDS;
            bl.IsPFExempted = obj.IsPFExempted == "Y" ? "N" : "Y"; 
            bl.IsESIExempted = obj.IsESIExempted == "Y" ? "N" : "Y"; 
            dal.InsertOrUpdate(bl);

            //Calculate Arrear
            for (int i = 0; i < obj.ArMonth; i++)
            {
                try
                {
                    DateTime dt = new DateTime(Convert.ToInt32(obj.MonthYear.Substring(3, 4)), Convert.ToInt32(obj.MonthYear.Substring(0, 2)), 1);
                    DateTime newdt = dt.AddMonths(i);
                    int year = newdt.Year;
                    int month = newdt.Month;
                    int days = System.DateTime.DaysInMonth(year, month);

                    string monthyear = month.ToString().PadLeft(2, '0') + "/" + year.ToString();

                    IMonthlySalary msal = mdal.GetEmpMonth(obj.EmpId, monthyear).SingleOrDefault();

                    double basicArrear = Math.Round((obj.Basic / days) * msal.PaidDays) - msal.Basic;
                    double hraArrear = Math.Round((obj.HRA / days) * msal.PaidDays) - msal.HRA;
                    double coneyvanceArrear = Math.Round((obj.Conveyance / days) * msal.PaidDays) - msal.Conveyance;
                    double medicalArrear = Math.Round((obj.Medical / days) * msal.PaidDays) - msal.Medical;

                    double EduAllowanceArrear = Math.Round((obj.EduAllowance / days) * msal.PaidDays) - msal.Medical;
                    double TelephoneReimbArrear = Math.Round((obj.Medical / days) * msal.PaidDays) - msal.Medical;
                    double CarRunningReimbArrear = Math.Round((obj.Medical / days) * msal.PaidDays) - msal.Medical;
                    double SatutoryBonusArrear = Math.Round((obj.Medical / days) * msal.PaidDays) - msal.Medical;
                    double OtherAllowanceArrear = Math.Round((obj.Medical / days) * msal.PaidDays) - msal.Medical;

                    double pfArrear = Math.Round((basicArrear * 12) / 100);
                    double esiArrear = 0;
                    double grossArrear = basicArrear + hraArrear + coneyvanceArrear + medicalArrear + EduAllowanceArrear + TelephoneReimbArrear + CarRunningReimbArrear + SatutoryBonusArrear + OtherAllowanceArrear;
                    if (obj.ESI != 0)
                    {
                        esiArrear = Math.Ceiling((grossArrear * bl.Employee.StatutoryCode.ESICont) / 100);
                    }
                    double netArrear = grossArrear - pfArrear - esiArrear;

                    ISalaryArrear sbl = new SalaryArrear();
                    sbl.Basic = basicArrear;
                    sbl.HRA = hraArrear;
                    sbl.Conveyance = coneyvanceArrear;
                    sbl.Medical = medicalArrear;
                    sbl.EduAllowance = EduAllowanceArrear;
                    sbl.TelephoneReimb = TelephoneReimbArrear;
                    sbl.CarRunningReimb = CarRunningReimbArrear;
                    sbl.SatutoryBonus = SatutoryBonusArrear;
                    sbl.OtherAllowance = OtherAllowanceArrear;
                    sbl.PF = pfArrear;
                    sbl.ESI = esiArrear;
                    sbl.MonthYear = monthyear;
                    sbl.EmpId = obj.EmpId;
                    aDal.InsertOrUpdate(sbl);
                }
                catch { };
            }

        }
        public override IList<SalaryMasterModel> GetAll()
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<SalaryMasterModel> model = AutoMapper.Mapper.Map<List<SalaryMasterModel>>(dal.GetAll());

            return model;
        }

        public IList<SalaryMasterModel> GetAll(int status)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<SalaryMasterModel> model = AutoMapper.Mapper.Map<List<SalaryMasterModel>>(dal.GetActive(status));

            return model;
        }

        public IList<SalaryMasterModel> GetAll(int compid,int branid)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<SalaryMasterModel> model = AutoMapper.Mapper.Map<List<SalaryMasterModel>>(dal.GetByCompBran(compid, branid));

            return model;
        }

        public IList<SalaryMasterModel> GetByBranch(int branid)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<SalaryMasterModel> model = AutoMapper.Mapper.Map<List<SalaryMasterModel>>(dal.GetByBranch(branid));

            return model;
        }

        public override SalaryMasterModel GetById(int id)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>();
            AutoMapper.Mapper.CreateMap<SalaryMaster, SalaryMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            SalaryMasterModel model = AutoMapper.Mapper.Map<SalaryMasterModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(SalaryMasterModel obj)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            ISalaryMaster bl = new SalaryMaster();
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
            bl.PF = obj.PF;
            bl.VPF = obj.VPF;
            bl.ESI = obj.ESI;
            bl.IsPFCelling = obj.IsPFCelling;
            bl.IsPFExempted = obj.IsPFExempted;
            bl.IsESIExempted = obj.IsESIExempted;
            dal.InsertOrUpdate(bl);
        }

        public void Insert(int empid)
        {
            SalaryMasterDAL dal = new SalaryMasterDAL();
            ISalaryMaster bl = new SalaryMaster();
            bl.Basic = 0;
            bl.DA = 0;
            bl.HRA = 0;
            bl.Conveyance = 0;
            bl.Medical = 0;
            bl.EduAllowance = 0;
            bl.TelephoneReimb = 0;
            bl.CarRunningReimb = 0;
            bl.SatutoryBonus = 0;
            bl.OtherAllowance = 0;
            bl.PF = 0;
            bl.VPF = 0;
            bl.ESI = 0;
            bl.TDS = 0;
            bl.IsPFCelling = "N";
            bl.IsPFExempted = "N";
            bl.IsESIExempted = "Y";
            dal.InsertOrUpdate(bl);
        }
    }
}