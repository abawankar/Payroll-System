using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Common;
using WebPayroll.DAL.Master;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.Master;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Domain.Interfaces.Master;

namespace WebPayroll.Models.Master
{
    public class EmployeeModel : Domain.Implementaion.Employee
    {
        public int CompId { get; set; }
        public int BranchId { get; set; }
        public int DeptId { get; set; }
        public int DesigId { get; set; }
        public int StatuId { get; set; }
        public string TotalYears
        {
            get
            {
                if (DOE == null) { 
                DateDifference d = new DateDifference(Convert.ToDateTime(DOJ), System.DateTime.Today);
                return d.ToString();
                    }
                else
                {
                    DateDifference d = new DateDifference(Convert.ToDateTime(DOJ), Convert.ToDateTime(DOE));
                    return d.ToString();
                }

            }
        }
        public string Aadhaar
        {
            get
            {
                if (KYC != null)
                {
                    foreach (var item in KYC)
                    {
                        if (item.DoxType == "A")
                            return item.DocumentNumber;
                    }
                }
                return "";
            }
        }
        public string PAN
        {
            get
            {
                if (KYC != null)
                {
                    foreach (var item in KYC)
                    {
                        if (item.DoxType == "P")
                            return item.DocumentNumber;
                    }
                }
                return "";
            }
        }
        public string BankAcc
        {
            get
            {
                if (KYC != null)
                {
                    foreach (var item in KYC)
                    {
                        if (item.DoxType == "B")
                            return item.DocumentNumber + " | " + item.Other;
                    }
                }
                return "";
            }
        }
    }

    public class EmployeeKYCModel:Domain.Implementaion.EmployeeKYC
    {
        public int EmpId { get; set; }
    }

    public class EmployeePastWorkingModel : Domain.Implementaion.Master.EmployeePastWorking
    {
        public string TotalYears
        {
            get
            {
                DateDifference d = new DateDifference(Convert.ToDateTime(DateTo), Convert.ToDateTime(DateFrom));
                return d.ToString();

            }
        }
        public int EmpId { get; set; }
    }

    public class EmployeeRepository : RepositoryModel<EmployeeModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(obj.Id);
            //bl.EmpCode = obj.EmpCode;
            bl.Name = obj.Name;
            bl.Gender = obj.Gender;
            bl.MarritalStatus = obj.MarritalStatus;
            bl.DOB = obj.DOB;
            bl.FatherOrHusbandName = obj.FatherOrHusbandName;
            bl.FNHFlag = obj.FNHFlag;
            bl.UAN = obj.UAN;
            bl.ESIIP = obj.ESIIP;
            bl.DOJ = obj.DOJ;
            bl.DOE = obj.DOE;
            bl.Status = obj.Status;
            bl.TranType = obj.TranType;
            bl.StatutoryCode = new StatutoryCode { Id = obj.StatuId };
            //bl.Company = new Company {Id=obj.CompId};
            //bl.Branch = new Branch { Id = obj.BranchId };
            bl.Department = new Department { Id = obj.DeptId };
            bl.Designation = new Designation { Id = obj.DesigId };
            dal.InsertOrUpdate(bl);
        }

        public override IList<EmployeeModel> GetAll()
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Branch.Id))
                .ForMember(dest => dest.DeptId, opt => opt.MapFrom(scr => scr.Department.Id))
                .ForMember(dest => dest.DesigId, opt => opt.MapFrom(scr => scr.Designation.Id))
                .ForMember(dest => dest.StatuId, opt => opt.MapFrom(scr => scr.StatutoryCode.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetAll().ToList());
            return model;
        }

        public override EmployeeModel GetById(int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Branch.Id))
                .ForMember(dest => dest.DeptId, opt => opt.MapFrom(scr => scr.Department.Id))
                .ForMember(dest => dest.DesigId, opt => opt.MapFrom(scr => scr.Designation.Id))
                .ForMember(dest => dest.StatuId, opt => opt.MapFrom(scr => scr.StatutoryCode.Id));
            EmployeeModel model = AutoMapper.Mapper.Map<EmployeeModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(EmployeeModel obj)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = new Employee();
            bl.EmpCode = obj.EmpCode;
            bl.Name = obj.Name;
            bl.Gender = obj.Gender;
            bl.MarritalStatus = obj.MarritalStatus;
            bl.DOB = obj.DOB;
            bl.FatherOrHusbandName = obj.FatherOrHusbandName;
            bl.FNHFlag = obj.FNHFlag;
            bl.UAN = obj.UAN;
            bl.ESIIP = obj.ESIIP;
            bl.DOJ = obj.DOJ;
            bl.Status = obj.Status;
            bl.TranType = obj.TranType;
            bl.Company = new Company { Id = obj.CompId };
            bl.Branch = new Branch { Id = obj.BranchId };
            bl.Department = new Department { Id = obj.DeptId };
            bl.Designation = new Designation { Id = obj.DesigId };
            bl.StatutoryCode = new StatutoryCode { Id = obj.StatuId };
            int empid = dal.Insert(bl);

            SalaryMasterDAL saldal = new SalaryMasterDAL();
            ISalaryMaster salary = new SalaryMaster();
            salary.Basic = 0;
            salary.DA = 0;
            salary.HRA = 0;
            salary.Conveyance = 0;
            salary.Medical = 0;
            salary.EduAllowance = 0;
            salary.TelephoneReimb = 0;
            salary.CarRunningReimb = 0;
            salary.SatutoryBonus = 0;
            salary.OtherAllowance = 0;
            salary.PF = 0;
            salary.VPF = 0;
            salary.ESI = 0;
            salary.IsPFCelling = "N";
            salary.IsPFExempted = "N";
            if(bl.ESIIP != null) {
                salary.IsESIExempted = bl.ESIIP.Length == 10 ? "N" : "Y";
            }
            salary.Employee = new Employee { Id = empid };
            saldal.InsertOrUpdate(salary);
        }

        public IList<EmployeeModel> GetByBranch(int branch)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>();
            AutoMapper.Mapper.CreateMap<Employee, EmployeeModel>()
                .ForMember(dest => dest.CompId, opt => opt.MapFrom(scr => scr.Company.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Branch.Id))
                .ForMember(dest => dest.DeptId, opt => opt.MapFrom(scr => scr.Department.Id))
                .ForMember(dest => dest.DesigId, opt => opt.MapFrom(scr => scr.Designation.Id))
                .ForMember(dest => dest.StatuId, opt => opt.MapFrom(scr => scr.StatutoryCode.Id));
            List<EmployeeModel> model = AutoMapper.Mapper.Map<List<EmployeeModel>>(dal.GetByBranch(branch).ToList());
            return model;
        }

    }

    public class EmployeeKYCRepository : RepositoryModel<EmployeeKYCModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(EmployeeKYCModel obj)
        {
            EmployeeKYCDAL dal = new EmployeeKYCDAL();
            IEmployeeKYC bl = dal.GetById(obj.Id);
            bl.DoxType = obj.DoxType;
            bl.NameonDox = obj.NameonDox;
            bl.DocumentNumber = obj.DocumentNumber;
            bl.Other = obj.Other;
            bl.IssueDate = obj.IssueDate;
            bl.Exipiry = obj.Exipiry;
            bl.Place = obj.Place;
            dal.InsertOrUpdate(bl);
        }

        public override IList<EmployeeKYCModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<EmployeeKYCModel> GetAll(int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<EmployeeKYC, EmployeeKYCModel>();
            List<EmployeeKYCModel> model = AutoMapper.Mapper.Map<List<EmployeeKYCModel>>(dal.GetById(id).KYC);
            return model;
        }

        public override EmployeeKYCModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(EmployeeKYCModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(EmployeeKYCModel obj, int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(id);
            bl.KYC.Add(new EmployeeKYC()
            {
                DoxType = obj.DoxType,
                NameonDox = obj.NameonDox,
                DocumentNumber = obj.DocumentNumber,
                Other = obj.Other,
                IssueDate = obj.IssueDate,
                Exipiry = obj.Exipiry,
                Place = obj.Place
            });
            dal.InsertOrUpdate(bl);
        }
    }

    public class EmployeePastWorkingRepository : RepositoryModel<EmployeePastWorkingModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(EmployeePastWorkingModel obj)
        {
            EmployeePastWorkingDAL dal = new EmployeePastWorkingDAL();
            IEmployeePastWorking bl = dal.GetById(obj.Id);
            bl.CompanyName = obj.CompanyName;
            bl.DateFrom = obj.DateFrom;
            bl.DateTo = obj.DateTo;
            bl.Rejoin = obj.Rejoin;
            bl.Comments = obj.Comments;
            dal.InsertOrUpdate(bl);
        }

        public override IList<EmployeePastWorkingModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<EmployeePastWorkingModel> GetAll(int empid)
        {
            EmployeeDAL dal = new EmployeeDAL();
            AutoMapper.Mapper.CreateMap<EmployeePastWorking, EmployeePastWorkingModel>();
            List<EmployeePastWorkingModel> model = AutoMapper.Mapper.Map<List<EmployeePastWorkingModel>>(dal.GetById(empid).PastWorkings);
            return model;
        }

        public override EmployeePastWorkingModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(EmployeePastWorkingModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(EmployeePastWorkingModel obj,int id)
        {
            EmployeeDAL dal = new EmployeeDAL();
            IEmployee bl = dal.GetById(id);
            bl.PastWorkings.Add(new EmployeePastWorking()
            {
                CompanyName = obj.CompanyName,
                DateFrom = obj.DateFrom,
                DateTo = obj.DateTo,
                Rejoin = obj.Rejoin,
                Comments = obj.Comments,
            });
            dal.InsertOrUpdate(bl);
        }
    }

}