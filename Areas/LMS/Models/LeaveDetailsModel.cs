using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.Common;
using WebPayroll.DAL.LMS;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.LMS;
using WebPayroll.Domain.Interfaces.LMS;
using WebPayroll.Models;

namespace WebPayroll.Areas.LMS.Models
{
    public class LeaveDetailsModel :Domain.Implementaion.LMS.LeaveDetails
    {
        public int EmpId { get; set; }
        public double Total {
            get {
                double t = JAN + FEB + MAR + APR + MAY + JUN + JUL + AUG + SEP + OCT + NOV + DEC;
                return t;
            }
        }
        public double Balance
        {
            get
            {
                double t = PaidLeave + Extra + UnPaid - Total;
                return t;
            }
        }
    }

    public class LeaveDetailsHeader
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime? DOJ { get; set; }
        public string TotalExp {
            get
            {
                DateDifference d = new DateDifference(Convert.ToDateTime(DOJ), System.DateTime.Today);
                return d.ToString();
            }
        }
        public double PaidLeave { get; set; }
        public double Extra { get; set; }
        public double Unpaid { get; set; }
        public double Total { get; set; }
        public double Balance { get {
                return PaidLeave + Extra + Unpaid - Total;
            } }
        public double Deducted { get; set; }

    }

    public class LeaveDetailsRepository : RepositoryModel<LeaveDetailsModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LeaveDetailsModel obj)
        {
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            ILeaveDetails bl = dal.GetById(obj.Id);
            bl.PaidLeave = obj.PaidLeave;
            bl.Extra = obj.Extra;
            bl.UnPaid = obj.UnPaid;
            bl.JAN = obj.JAN;
            bl.FEB = obj.FEB;
            bl.MAR = obj.MAR;
            bl.APR = obj.APR;
            bl.MAY = obj.MAY;
            bl.JUN = obj.JUN;
            bl.JUL = obj.JUL;
            bl.AUG = obj.AUG;
            bl.SEP = obj.SEP;
            bl.OCT = obj.OCT;
            bl.NOV = obj.NOV;
            bl.DEC = obj.DEC;
            dal.InsertOrUpdate(bl);
        }

        public override IList<LeaveDetailsModel> GetAll()
        {
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            AutoMapper.Mapper.CreateMap<LeaveDetails, LeaveDetailsModel>();
            AutoMapper.Mapper.CreateMap<LeaveDetails, LeaveDetailsModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<LeaveDetailsModel> model = AutoMapper.Mapper.Map<List<LeaveDetailsModel>>(dal.GetAll());

            return model;
        }

        public IList<LeaveDetailsModel> GetActiveByBranch(int branchid,int yr)
        {
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            AutoMapper.Mapper.CreateMap<LeaveDetails, LeaveDetailsModel>();
            AutoMapper.Mapper.CreateMap<LeaveDetails, LeaveDetailsModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<LeaveDetailsModel> model = AutoMapper.Mapper.Map<List<LeaveDetailsModel>>(dal.GetActiveByBranch(branchid,yr));

            return model;
        }

        public override LeaveDetailsModel GetById(int id)
        {
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            AutoMapper.Mapper.CreateMap<LeaveDetails, LeaveDetailsModel>();
            AutoMapper.Mapper.CreateMap<LeaveDetails, LeaveDetailsModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            LeaveDetailsModel model = AutoMapper.Mapper.Map<LeaveDetailsModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(LeaveDetailsModel obj)
        {
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            ILeaveDetails bl = new LeaveDetails();
            bl.Year=obj.Year;
            bl.PaidLeave=obj.PaidLeave;
            bl.Extra=obj.Extra;
            bl.UnPaid= obj.UnPaid;
            bl.JAN=obj.JAN;
            bl.FEB=obj.FEB;
            bl.MAR=obj.MAR;
            bl.APR=obj.APR;
            bl.MAY=obj.MAY;
            bl.JUN=obj.JUN;
            bl.JUL = obj.JUL;
            bl.AUG=obj.AUG;
            bl.SEP=obj.SEP;
            bl.OCT=obj.OCT;
            bl.NOV=obj.NOV;
            bl.DEC=obj.DEC;
            bl.Employee = new Employee {Id=obj.EmpId };
            dal.InsertOrUpdate(bl);
        }
    }
}