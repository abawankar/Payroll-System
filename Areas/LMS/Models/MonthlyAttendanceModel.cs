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
    public class MonthlyAttendanceModel:Domain.Implementaion.LMS.MonthlyAttendance
    {
        public int EmpId { get; set; }
        public int BranchId { get; set; }
    }

    public class MonthlyAttendanceRepository : RepositoryModel<MonthlyAttendanceModel>
    {
        public override bool Delete(int id)
        {
            MonthlyAttendanceDAL dal = new MonthlyAttendanceDAL();
            IMonthlyAttendance bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public override void Edit(MonthlyAttendanceModel obj)
        {
            MonthlyAttendanceDAL dal = new MonthlyAttendanceDAL();
            IMonthlyAttendance bl = dal.GetById(obj.Id);
            bl.Date = obj.Date;
            bl.Days = Convert.ToDateTime(obj.Date).Day;
            bl.Month = Convert.ToDateTime(obj.Date).Month;
            bl.Year = Convert.ToDateTime(obj.Date).Year;
            bl.LeaveType = obj.LeaveType;
            dal.InsertOrUpdate(bl);
        }

        public override IList<MonthlyAttendanceModel> GetAll()
        {
            MonthlyAttendanceDAL dal = new MonthlyAttendanceDAL();
            AutoMapper.Mapper.CreateMap<MonthlyAttendance, MonthlyAttendanceModel>();
            AutoMapper.Mapper.CreateMap<MonthlyAttendance, MonthlyAttendanceModel>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<MonthlyAttendanceModel> model = AutoMapper.Mapper.Map<List<MonthlyAttendanceModel>>(dal.GetAll().ToList());
            return model;
        }
        public IList<MonthlyAttendanceModel> GetByEmp(int empid)
        {
            MonthlyAttendanceDAL dal = new MonthlyAttendanceDAL();
            AutoMapper.Mapper.CreateMap<MonthlyAttendance, MonthlyAttendanceModel>();
            AutoMapper.Mapper.CreateMap<MonthlyAttendance, MonthlyAttendanceModel>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<MonthlyAttendanceModel> model = AutoMapper.Mapper.Map<List<MonthlyAttendanceModel>>(dal.GetByEmployee(empid).ToList());
            return model;
        }

        public override MonthlyAttendanceModel GetById(int id)
        {
            MonthlyAttendanceDAL dal = new MonthlyAttendanceDAL();
            AutoMapper.Mapper.CreateMap<MonthlyAttendance, MonthlyAttendanceModel>();
            AutoMapper.Mapper.CreateMap<MonthlyAttendance, MonthlyAttendanceModel>()
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(scr => scr.Employee.Branch.Id))
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            MonthlyAttendanceModel model = AutoMapper.Mapper.Map<MonthlyAttendanceModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(MonthlyAttendanceModel obj)
        {
            MonthlyAttendanceDAL dal = new MonthlyAttendanceDAL();
            IMonthlyAttendance bl = new MonthlyAttendance();
            bl.Date = obj.Date;
            bl.Days = Convert.ToDateTime(obj.Date).Day;
            bl.Month = Convert.ToDateTime(obj.Date).Month;
            bl.Year = Convert.ToDateTime(obj.Date).Year;
            bl.LeaveType = obj.LeaveType;
            bl.Employee = new Employee { Id = obj.EmpId };
            dal.InsertOrUpdate(bl);
        }
    }
}