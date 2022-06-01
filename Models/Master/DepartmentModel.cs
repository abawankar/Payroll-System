using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class DepartmentModel: Domain.Implementaion.Department
    {
    }

    public class DepartmentRepository : RepositoryModel<DepartmentModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(DepartmentModel obj)
        {
            DepartmentDAL dal = new DepartmentDAL();
            IDepartment bl = dal.GetById(obj.Id);
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }

        public override IList<DepartmentModel> GetAll()
        {
            DepartmentDAL dal = new DepartmentDAL();
            AutoMapper.Mapper.CreateMap<Department, DepartmentModel>();
            List<DepartmentModel> model = AutoMapper.Mapper.Map<List<DepartmentModel>>(dal.GetAll());
            return model;
        }

        public override DepartmentModel GetById(int id)
        {
            DepartmentDAL dal = new DepartmentDAL();
            AutoMapper.Mapper.CreateMap<Department, DepartmentModel>();
            DepartmentModel model = AutoMapper.Mapper.Map<DepartmentModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(DepartmentModel obj)
        {
            DepartmentDAL dal = new DepartmentDAL();
            IDepartment bl = new Department();
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }
    }
}