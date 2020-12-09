using System;
using System.Collections.Generic;
using UserService.IService;
using UserService.Model;

namespace UserService.Service
{
    public class UserService : IUserService
    {
        #region DataInit
        private List<User> _UserList = new List<User>()
        {
            new User()
            {
                Id=1,
                Account="Administrator",
                Email="57265177@qq.com",
                Name="Eleven",
                Password="1234567890",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
             new User()
            {
                Id=1,
                Account="Apple",
                Email="57265177@qq.com",
                Name="Apple",
                Password="1234567890",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
              new User()
            {
                Id=1,
                Account="Cole",
                Email="57265177@qq.com",
                Name="Cole",
                Password="1234567890",
                LoginTime=DateTime.Now,
                Role="Admin"
            },
        };
        #endregion
        public User FindSingle(int id)
        {
            return this._UserList.Find(x => x.Id == id);
        }

        public IEnumerable<User> FindUsers()
        {
            return this._UserList;
        }
    }
}
