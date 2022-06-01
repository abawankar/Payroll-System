using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;
using WebPayroll.Models;

namespace WebPayroll.Areas.LMS.Models
{
    public class LeaveTypeModel:Domain.Implementaion.LeaveType
    {
    }

    public class LeaveTypeRepository : RepositoryModel<LeaveTypeModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LeaveTypeModel obj)
        {
            LeaveTypeDAL dal = new LeaveTypeDAL();
            ILeaveType bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }

        public override IList<LeaveTypeModel> GetAll()
        {
            LeaveTypeDAL dal = new LeaveTypeDAL();
            AutoMapper.Mapper.CreateMap<LeaveType, LeaveTypeModel>();
            List<LeaveTypeModel> model = AutoMapper.Mapper.Map<List<LeaveTypeModel>>(dal.GetAll());
            return model;
        }

        public override LeaveTypeModel GetById(int id)
        {
            LeaveTypeDAL dal = new LeaveTypeDAL();
            AutoMapper.Mapper.CreateMap<LeaveType, LeaveTypeModel>();
            LeaveTypeModel model = AutoMapper.Mapper.Map<LeaveTypeModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(LeaveTypeModel obj)
        {
            LeaveTypeDAL dal = new LeaveTypeDAL();
            ILeaveType bl = new LeaveType();
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }
    }
}