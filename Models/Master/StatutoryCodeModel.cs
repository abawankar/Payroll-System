using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class StatutoryCodeModel: Domain.Implementaion.StatutoryCode
    {
    }

    public class StatutoryCodeRepository : RepositoryModel<StatutoryCodeModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(StatutoryCodeModel obj)
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            IStatutoryCode bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.PFCont = obj.PFCont;
            bl.ESICont = obj.ESICont;
            bl.PFCelling = obj.PFCelling;
            bl.ESICelling = obj.ESICelling;
            dal.InsertOrUpdate(bl);
        }

        public override IList<StatutoryCodeModel> GetAll()
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            AutoMapper.Mapper.CreateMap<StatutoryCode, StatutoryCodeModel>();
            List<StatutoryCodeModel> model = AutoMapper.Mapper.Map<List<StatutoryCodeModel>>(dal.GetAll());
            return model;
        }

        public override StatutoryCodeModel GetById(int id)
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            AutoMapper.Mapper.CreateMap<StatutoryCode, StatutoryCodeModel>();
            StatutoryCodeModel model = AutoMapper.Mapper.Map<StatutoryCodeModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(StatutoryCodeModel obj)
        {
            StatutoryCodeDAL dal = new StatutoryCodeDAL();
            IStatutoryCode bl = new StatutoryCode();
            bl.Code = obj.Code;
            bl.PFCont = obj.PFCont;
            bl.ESICont = obj.ESICont;
            bl.PFCelling = obj.PFCelling;
            bl.ESICelling = obj.ESICelling;
            dal.InsertOrUpdate(bl);
        }
    }
}