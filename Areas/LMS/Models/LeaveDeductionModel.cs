using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.LMS;
using WebPayroll.Domain.Implementaion.LMS;
using WebPayroll.Domain.Interfaces.LMS;
using WebPayroll.Models;

namespace WebPayroll.Areas.LMS.Models
{
    public class LeaveDeductionModel:Domain.Implementaion.LMS.LeaveDeduction
    {
        public double Total
        {
            get
            {
                double t = JAN + FEB + MAR + APR + MAY + JUN + JUL + AUG + SEP + OCT + NOV + DEC;
                return t;
            }
        }
    }

    public class LeaveDeductionRepository : RepositoryModel<LeaveDeductionModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(LeaveDeductionModel obj)
        {
            LeaveDeductionDAL dal = new LeaveDeductionDAL();
            ILeaveDeduction bl = dal.GetById(obj.Id);
            bl.JAN = obj.JAN;
            bl.FEB = obj.FEB;
            bl.MAR = obj.MAR;
            bl.APR = obj.APR;
            bl.MAY = obj.MAY;
            bl.JUN = obj.JUN;
            bl.AUG = obj.AUG;
            bl.SEP = obj.SEP;
            bl.OCT = obj.OCT;
            bl.NOV = obj.NOV;
            bl.DEC = obj.DEC;
            dal.InsertOrUpdate(bl);
        }

        public override IList<LeaveDeductionModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<LeaveDeductionModel> GetAll(int id)
        {
            LeaveDetailsDAL dal = new LeaveDetailsDAL();
            AutoMapper.Mapper.CreateMap<LeaveDeduction, LeaveDeductionModel>();
            List<LeaveDeductionModel> model = AutoMapper.Mapper.Map<List<LeaveDeductionModel>>(dal.GetById(id).Deduction);

            return model;
        }

        public override LeaveDeductionModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Insert(LeaveDeductionModel obj)
        {
            throw new NotImplementedException();
        }
    }
}