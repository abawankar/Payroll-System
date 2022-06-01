using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.LMS;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Implementaion.LMS;
using WebPayroll.Domain.Interfaces.LMS;
using WebPayroll.Models;

namespace WebPayroll.Areas.LMS.Models
{
    public class LeaveApplicationModel:Domain.Implementaion.LMS.LeaveApplication
    {
        public int EmpId { get; set; }
        public int LeaveTypeId { get; set; }
        public int BranchId { get; set; }
    }

    public class LeaveApplicationRepository : RepositoryModel<LeaveApplicationModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LeaveApplicationModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<LeaveApplicationModel> GetAll()
        {
            LeaveApplicationDAL dal = new LeaveApplicationDAL();
            AutoMapper.Mapper.CreateMap<LeaveApplication, LeaveApplicationModel>();
            AutoMapper.Mapper.CreateMap<LeaveApplication, LeaveApplicationModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.LeaveTypeId, opt => opt.MapFrom(scr => scr.LeaveType.Id));
            List<LeaveApplicationModel> model = AutoMapper.Mapper.Map<List<LeaveApplicationModel>>(dal.GetAll());

            return model;
        }

        public override LeaveApplicationModel GetById(int id)
        {
            LeaveApplicationDAL dal = new LeaveApplicationDAL();
            AutoMapper.Mapper.CreateMap<LeaveApplication, LeaveApplicationModel>();
            AutoMapper.Mapper.CreateMap<LeaveApplication, LeaveApplicationModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.LeaveTypeId, opt => opt.MapFrom(scr => scr.LeaveType.Id));
            LeaveApplicationModel model = AutoMapper.Mapper.Map<LeaveApplicationModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(LeaveApplicationModel obj)
        {
            LeaveApplicationDAL dal = new LeaveApplicationDAL();
            ILeaveApplication bl = new LeaveApplication();
            bl.Date = System.DateTime.Now;
            bl.DateFrom = obj.DateFrom;
            bl.PeriodFrom = obj.PeriodFrom;
            bl.DateTo = obj.DateTo;
            bl.PeriodTo = obj.PeriodTo;
            bl.LeaveReason = obj.LeaveReason;

            bl.Employee = new Employee { Id = obj.EmpId };
            bl.LeaveType = new LeaveType { Id = obj.LeaveTypeId };

            TimeSpan diff = bl.DateTo - bl.DateFrom;
            double days = (diff.TotalDays)+1;
            if (bl.PeriodFrom == 2) days = days - 0.5;
            if (bl.PeriodTo == 2) days = days - 0.5;

            bl.TotalLeave = days;
            bl.Status = 1;

            dal.InsertOrUpdate(bl);
        }
    }
}