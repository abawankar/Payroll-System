using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace WebPayroll.Models
{

    public abstract class RepositoryModel<T>
    {
        public abstract T GetById(int id);
        public abstract IList<T> GetAll();
        public abstract void Edit(T obj);
        public abstract void Insert(T obj);
        public abstract bool Delete(int id);
    }

    public static class MyExtension
    {
        public static string Get8Digits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            return String.Format("{0:D8}", random);
        }
    }
    

}