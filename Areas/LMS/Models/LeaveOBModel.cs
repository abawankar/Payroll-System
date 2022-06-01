using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Models;

namespace WebPayroll.Areas.LMS.Models
{
    public class LeaveOBModel:Domain.Implementaion.LeaveOB
    {
        public int EmpId { get; set; }
        public int LeaveTypeId { get; set; }
        public int BranchId { get; set; }
    }

    public class LeaveOBRepository : RepositoryModel<LeaveOBModel>
    {
        public override bool Delete(int id)
        {
            LeaveOBDAL dal = new LeaveOBDAL();
            ILeaveOB bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public override void Edit(LeaveOBModel obj)
        {
            LeaveOBDAL dal = new LeaveOBDAL();
            ILeaveOB bl = dal.GetById(obj.Id);
            bl.Year = obj.Year;
            bl.BalanceLeave = obj.BalanceLeave;
            bl.LeaveType = new LeaveType { Id = obj.LeaveTypeId };
            dal.InsertOrUpdate(bl);
        }

        public override IList<LeaveOBModel> GetAll()
        {
            LeaveOBDAL dal = new LeaveOBDAL();
            AutoMapper.Mapper.CreateMap<LeaveOB, LeaveOBModel>();
            AutoMapper.Mapper.CreateMap<LeaveOB, LeaveOBModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.LeaveTypeId, opt => opt.MapFrom(scr => scr.LeaveType.Id));
            List<LeaveOBModel> model = AutoMapper.Mapper.Map<List<LeaveOBModel>>(dal.GetAll());

            return model;
        }

        public IList<LeaveOBModel> IsLeaveAdded(int empid, int leave, int year)
        {
            LeaveOBDAL dal = new LeaveOBDAL();
            AutoMapper.Mapper.CreateMap<LeaveOB, LeaveOBModel>();
            AutoMapper.Mapper.CreateMap<LeaveOB, LeaveOBModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.LeaveTypeId, opt => opt.MapFrom(scr => scr.LeaveType.Id));
            List<LeaveOBModel> model = AutoMapper.Mapper.Map<List<LeaveOBModel>>(dal.IsLeaveAdded(empid,leave,year));

            return model;
        }

        public override LeaveOBModel GetById(int id)
        {
            LeaveOBDAL dal = new LeaveOBDAL();
            AutoMapper.Mapper.CreateMap<LeaveOB, LeaveOBModel>();
            AutoMapper.Mapper.CreateMap<LeaveOB, LeaveOBModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.LeaveTypeId, opt => opt.MapFrom(scr => scr.LeaveType.Id));
            LeaveOBModel model = AutoMapper.Mapper.Map<LeaveOBModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(LeaveOBModel obj)
        {
            LeaveOBDAL dal = new LeaveOBDAL();
            ILeaveOB bl = new LeaveOB();
            bl.BalanceLeave = obj.BalanceLeave;
            bl.Year = obj.Year;
            bl.Employee = new Employee { Id = obj.EmpId };
            bl.LeaveType = new LeaveType { Id = obj.LeaveTypeId };
            dal.InsertOrUpdate(bl);
        }
    }
}