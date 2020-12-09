using System;
using System.Collections.Generic;
using UserService.Model;

namespace UserService.IService
{
    public interface IUserService
    {
        User FindSingle(int id);

        IEnumerable<User> FindUsers();
    }
}
