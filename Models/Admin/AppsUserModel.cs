using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebPayroll.DAL.Master;
using WebPayroll.Domain.Implementaion;
using WebPayroll.Domain.Interfaces;

namespace WebPayroll.Models.Admin
{
    public class AppsUserModel : AppsUser
    {
    }

    public class AppsUserRepository : RepositoryModel<AppsUserModel>
    {
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Edit(AppsUserModel obj)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = dal.GetById(obj.Id);
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.Status = obj.Status;
            dal.InsertOrUpdate(bl);
        }

        public override IList<AppsUserModel> GetAll()
        {
            AppsUserDAL dal = new AppsUserDAL();
            AutoMapper.Mapper.CreateMap<AppsUser, AppsUserModel>();
            List<AppsUserModel> model = AutoMapper.Mapper.Map<List<AppsUserModel>>(dal.GetAll());

            return model;
        }

        public override AppsUserModel GetById(int id)
        {
            AppsUserDAL dal = new AppsUserDAL();
            AutoMapper.Mapper.CreateMap<AppsUser, AppsUserModel>();
            AppsUserModel model = AutoMapper.Mapper.Map<AppsUserModel>(dal.GetById(id));

            return model;
        }

        public override void Insert(AppsUserModel obj)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = new AppsUser();
            bl.Code = obj.Code;
            bl.Name = obj.Name;
            bl.AppLogin = obj.AppLogin;
            bl.Status = 2;
            dal.InsertOrUpdate(bl);

        }

        public static void AddEmpRights(int id, string rightList)
        {
            AppsUserDAL dal = new AppsUserDAL();
            dal.AddRights(id, rightList);
        }

        public static int GetByUserName(string username)
        {
            return AppsUserDAL.GetByAppLogin(username).Id;
        }

        public bool IsUserExist(string username)
        {
            return AppsUserDAL.IsUserExist(username);
        }

        public static void DeleteRights(int empId, int RightId)
        {
            AppsUserDAL dal = new AppsUserDAL();
            IAppsUser bl = dal.GetById(empId);

            UserRightsDAL cdal = new UserRightsDAL();
            IUserRight rights = cdal.GetById(RightId);

            int i = bl.UserRight.IndexOf(rights);
            bl.UserRight.RemoveAt(i);
            dal.InsertOrUpdate(bl);
        }

        public static string[] LoginList()
        {
            AppsUserDAL dal = new AppsUserDAL();
            return dal.GetAll().Select(x => x.AppLogin).ToArray();
        }

        public static string[] RightList(string username)
        {
            var data = AppsUserDAL.GetByAppLogin(username.Trim()).UserRight.Select(x => x.Code).ToArray();
            return data;
        }


    }

}