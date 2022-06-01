using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class LoanCRTranModel : Domain.Implementaion.LoanCRTran
    {
    }

    public class LoanDRTranModel : Domain.Implementaion.LoanDRTran
    {
    }

    public class LoanCRTranRepository : RepositoryModel<LoanCRTranModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LoanCRTranModel obj)
        {
            LoanCRTranDAL dal = new LoanCRTranDAL();
            ILoanCRTran bl = dal.GetById(obj.Id);
            bl.Date = obj.Date;
            bl.Amount = obj.Amount;
            bl.Comments = obj.Comments;
            dal.InsertOrUpdate(bl);
        }

        public override IList<LoanCRTranModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<LoanCRTranModel> GetAll(int loanid)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            AutoMapper.Mapper.CreateMap<LoanCRTran, LoanCRTranModel>();
            List<LoanCRTranModel> model = AutoMapper.Mapper.Map<List<LoanCRTranModel>>(dal.GetById(loanid).LoanCRTran);
            return model;
        }

        public override LoanCRTranModel GetById(int id)
        {
            LoanCRTranDAL dal = new LoanCRTranDAL();
            AutoMapper.Mapper.CreateMap<LoanCRTran, LoanCRTranModel>();
            LoanCRTranModel model = AutoMapper.Mapper.Map<LoanCRTranModel>(dal.GetById(id));
            return model;
        }

        public override void Insert(LoanCRTranModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(LoanCRTranModel obj,int loanid)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            ILoanMaster bl = dal.GetById(loanid);
            ILoanCRTran t = new LoanCRTran();
            t.Date = obj.Date;
            t.Amount = obj.Amount;
            t.Comments = obj.Comments;
            bl.LoanCRTran.Add(t);
            dal.InsertOrUpdate(bl);
        }
    }

    public class LoanDRTranRepository : RepositoryModel<LoanDRTranModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LoanDRTranModel obj)
        {
            LoanDRTranDAL dal = new LoanDRTranDAL();
            ILoanDRTran bl = dal.GetById(obj.Id);
            bl.Date = obj.Date;
            bl.Amount = obj.Amount;
            bl.Comments = obj.Comments;
            if(bl.PaidBy != "S")
                bl.PaidBy = obj.PaidBy;

            dal.InsertOrUpdate(bl);
        }

        public override IList<LoanDRTranModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<LoanDRTranModel> GetAll(int loanid)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            AutoMapper.Mapper.CreateMap<LoanDRTran, LoanDRTranModel>();
            List<LoanDRTranModel> model = AutoMapper.Mapper.Map<List<LoanDRTranModel>>(dal.GetById(loanid).LoanDRTran);
            return model;
        }

        public override LoanDRTranModel GetById(int id)
        {
            LoanDRTranDAL dal = new LoanDRTranDAL();
            AutoMapper.Mapper.CreateMap<LoanDRTran, LoanDRTranModel>();
            LoanDRTranModel model = AutoMapper.Mapper.Map<LoanDRTranModel>(dal.GetById(id));
            return model;
        }

        public ILoanDRTran GetMonthDRId(int empid,string month)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            var data = dal.GetByEmpId(empid);
            ILoanDRTran bl;
            if (data != null)
            {
                bl = data.LoanDRTran.Where(x => x.PaidMonth == month && x.PaidBy=="S").SingleOrDefault();
                if(bl== null)
                { bl = new LoanDRTran(); }
            }
            else
            {
                bl = new LoanDRTran();
            }
            return bl;
        }

        public override void Insert(LoanDRTranModel obj)
        {
            throw new NotImplementedException();
        }

        public void Insert(LoanDRTranModel obj, int loanid)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            ILoanMaster bl = dal.GetById(loanid);

            ILoanDRTran t = new LoanDRTran();
            t.Date = obj.Date;
            t.Amount = obj.Amount;
            t.PaidBy = obj.PaidBy;
            t.PaidMonth = obj.PaidMonth;
            t.Comments = obj.Comments;
            bl.LoanDRTran.Add(t);
            dal.InsertOrUpdate(bl);
        }
    }


}