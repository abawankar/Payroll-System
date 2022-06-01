using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class DesignationModel : Domain.Implementaion.Designation
    {
    }

    public class DesignationRepository : RepositoryModel<DesignationModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(DesignationModel obj)
        {
            DesignationDAL dal = new DesignationDAL();
            IDesignation bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            bl.PaidLeave = obj.PaidLeave;
            dal.InsertOrUpdate(bl);
        }

        public override IList<DesignationModel> GetAll()
        {
            DesignationDAL dal = new DesignationDAL();
            AutoMapper.Mapper.CreateMap<Designation, DesignationModel>();
            List<DesignationModel> model = AutoMapper.Mapper.Map<List<DesignationModel>>(dal.GetAll());
            return model;
        }

        public override DesignationModel GetById(int id)
        {
            DesignationDAL dal = new DesignationDAL();
            AutoMapper.Mapper.CreateMap<Designation, DesignationModel>();
            DesignationModel model = AutoMapper.Mapper.Map<DesignationModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(DesignationModel obj)
        {
            DesignationDAL dal = new DesignationDAL();
            IDesignation bl = new Designation();
            bl.Name = obj.Name;
            bl.PaidLeave = obj.PaidLeave;
            dal.InsertOrUpdate(bl);
        }
    }
}