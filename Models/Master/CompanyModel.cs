using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class CompanyModel:Company
    {
    }

    public class CompanyRepository : RepositoryModel<CompanyModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(CompanyModel obj)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.PAN = obj.PAN;
            bl.GST = obj.GST;
            bl.Address = obj.Address;
            bl.CIN = obj.CIN;
            bl.EstablishmentCode = obj.EstablishmentCode;
            bl.ESIRegistrationNumber = obj.ESIRegistrationNumber;
            dal.InsertOrUpdate(bl);
        }

        public override IList<CompanyModel> GetAll()
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            List<CompanyModel> model = AutoMapper.Mapper.Map<List<CompanyModel>>(dal.GetAll());
            return model;
        }

        public override CompanyModel GetById(int id)
        {
            CompanyDAL dal = new CompanyDAL();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            CompanyModel model = AutoMapper.Mapper.Map<CompanyModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(CompanyModel obj)
        {
            CompanyDAL dal = new CompanyDAL();
            ICompany bl = new Company();
            bl.Code=obj.Code;
            bl.Name=obj.Name;
            bl.PAN=obj.PAN;
            bl.GST=obj.GST;
            bl.Address=obj.Address;
            bl.CIN=obj.CIN;
            bl.EstablishmentCode=obj.EstablishmentCode;
            bl.ESIRegistrationNumber = obj.ESIRegistrationNumber;
            dal.InsertOrUpdate(bl);
        }
    }
}