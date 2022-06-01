using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Master
{
    public class LoanMasterModel : Domain.Implementaion.LoanMaster
    {
        public int EmpId { get; set; }
        public string Branch { get; set; }
        string _paidby = null;
        public string PaidBy {
            get {
                if (_paidby == null) _paidby = "S";
                return _paidby;
            }
            set
            {
                _paidby = value;
            }
        }
        public double LoanAmount { get; set; }
        public double CRAmount {
            get { return LoanCRTran!= null? LoanCRTran.Sum(x => x.Amount):0; }
        }
        public double DRAmount
        {
            get { return LoanDRTran!= null?LoanDRTran.Sum(x => x.Amount):0; }
        }
        public double BalanceAmount
        {
            get { return CRAmount-DRAmount; }
        }
    }

    public class LoanMasterRepository : RepositoryModel<LoanMasterModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LoanMasterModel obj)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            ILoanMaster bl = dal.GetById(obj.Id);
            bl.Type = obj.Type;
            bl.Date = obj.Date;
            bl.PaymentMode = obj.PaymentMode;
            bl.DednFrom = obj.DednFrom;
            bl.DednAmount = obj.DednAmount;
            bl.Comments = obj.Comments;
            bl.Status = obj.Status;
            dal.InsertOrUpdate(bl);
        }

        public override IList<LoanMasterModel> GetAll()
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            AutoMapper.Mapper.CreateMap<LoanMaster, LoanMasterModel>();
            AutoMapper.Mapper.CreateMap<LoanMaster, LoanMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(scr => scr.Employee.Branch.Code));
            List<LoanMasterModel> model = AutoMapper.Mapper.Map<List<LoanMasterModel>>(dal.GetAll());

            return model;
        }

        public override LoanMasterModel GetById(int id)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            AutoMapper.Mapper.CreateMap<LoanMaster, LoanMasterModel>();
            AutoMapper.Mapper.CreateMap<LoanMaster, LoanMasterModel>()
                .ForMember(dest => dest.EmpId, opt => opt.MapFrom(scr => scr.Employee.Id))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(scr => scr.Employee.Branch.Code));
            LoanMasterModel model = AutoMapper.Mapper.Map<LoanMasterModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(LoanMasterModel obj)
        {
            LoanMasterDAL dal = new LoanMasterDAL();
            ILoanMaster bl = new LoanMaster();
            bl.LoanCode = MyExtension.Get8Digits();
            bl.Type = obj.Type;
            bl.Date = obj.Date;
            bl.PaymentMode = obj.PaymentMode;
            bl.DednFrom = obj.DednFrom;
            bl.DednAmount = obj.DednAmount;
            bl.Comments = obj.Comments;
            bl.Status =1;
            bl.Employee = new Employee { Id = obj.EmpId };

            ILoanCRTran t = new LoanCRTran();
            t.Date = obj.Date;
            t.Amount = obj.LoanAmount;
            t.Comments = obj.Comments;
            bl.LoanCRTran.Add(t);

            dal.InsertOrUpdate(bl);
        }
    }
}