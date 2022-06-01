using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Admin
{
    public class UserRightModel : UserRight
    {
    }

    public class UserRightRepository : RepositoryModel<UserRightModel>
    {

        public override UserRightModel GetById(int id)
        {
            UserRightsDAL dal = new UserRightsDAL();
            AutoMapper.Mapper.CreateMap<UserRight, UserRightModel>();
            UserRightModel model = AutoMapper.Mapper.Map<UserRightModel>(dal.GetById(id));

            return model;
        }

        public override IList<UserRightModel> GetAll()
        {
            UserRightsDAL dal = new UserRightsDAL();
            AutoMapper.Mapper.CreateMap<UserRight, UserRightModel>();
            List<UserRightModel> model = AutoMapper.Mapper.Map<List<UserRightModel>>(dal.GetAll());

            return model;
        }

        public override void Edit(UserRightModel obj)
        {
            UserRightsDAL dal = new UserRightsDAL();
            IUserRight bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.MnuName = obj.MnuName;
            bl.TableName = obj.TableName;
            bl.Operation = obj.Operation;
            bl.Description = obj.Description;
            dal.InsertOrUpdate(bl);
        }

        public override void Insert(UserRightModel obj)
        {
            UserRightsDAL dal = new UserRightsDAL();
            IUserRight bl = new UserRight();
            bl.Code = obj.Code;
            bl.MnuName = obj.MnuName;
            bl.TableName = obj.TableName;
            bl.Operation = obj.Operation;
            bl.Description = obj.Description;

            dal.InsertOrUpdate(bl);
        }

        public override bool Delete(int id)
        {
            UserRightsDAL dal = new UserRightsDAL();
            IUserRight bl = dal.GetById(id);
            return dal.Delete(bl);
        }

        public static string[] RightList(string username)
        {
            //var data = AppsUserDAL.GetByAppLogin(username.Trim()).UserRight.Select(x => x.Code).DefaultIfEmpty().ToArray();
            var data = AppsUserDAL.GetByAppLogin(username.Trim());
            if (data.UserRight != null)
                return data.UserRight.Select(x => x.Code).ToArray();
            else
                return new string[] { };
        }
    }
}