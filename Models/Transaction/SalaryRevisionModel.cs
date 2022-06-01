using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Transaction;
using WebPayroll.Domain.Implementaion;

namespace WebPayroll.Models.Transaction
{
    public class SalaryRevisionModel:Domain.Implementaion.SalaryRevision
    {
        public int EmpId { get; set; }
    }

    public class TempSalaryRevModel
    {
        public int SalaryId { get; set; }
        public int EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public double Basic { get; set; }
        public double HRA { get; set; }
        public double Conveyance { get; set; }
        public double Medical { get; set; }
        public double PF { get; set; }
        public double ESI { get; set; }
        public string MonthYear { get; set; }
        public int ArrearMonth { get; set; }
    }

    public class SalaryRevisionRepository : RepositoryModel<SalaryRevisionModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(SalaryRevisionModel obj)
        {
            throw new NotImplementedException();
        }

        public override IList<SalaryRevisionModel> GetAll()
        {
            SalaryRevisionDAL dal = new SalaryRevisionDAL();
            AutoMapper.Mapper.CreateMap<SalaryRevision, SalaryRevisionModel>();
            AutoMapper.Mapper.CreateMap<SalaryRevision, SalaryRevisionModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id));
            List<SalaryRevisionModel> model = AutoMapper.Mapper.Map<List<SalaryRevisionModel>>(dal.GetAll().ToList());
            return model;
        }

        public override SalaryRevisionModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(SalaryRevisionModel obj)
        {
            throw new NotImplementedException();
        }
    }
}