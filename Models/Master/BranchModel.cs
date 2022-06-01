using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class BranchModel:Domain.Implementaion.Branch
    {

    }

    public class BranchRepository : RepositoryModel<BranchModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(BranchModel obj)
        {
            BranchDAL dal = new BranchDAL();
            IBranch bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }

        public override IList<BranchModel> GetAll()
        {
            BranchDAL dal = new BranchDAL();
            AutoMapper.Mapper.CreateMap<Branch, BranchModel>();
            List<BranchModel> model = AutoMapper.Mapper.Map<List<BranchModel>>(dal.GetAll());
            return model;
        }

        public override BranchModel GetById(int id)
        {
            BranchDAL dal = new BranchDAL();
            AutoMapper.Mapper.CreateMap<Branch, BranchModel>();
            BranchModel model = AutoMapper.Mapper.Map<BranchModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(BranchModel obj)
        {
            BranchDAL dal = new BranchDAL();
            IBranch bl = new Branch();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            dal.InsertOrUpdate(bl);
        }
    }
}