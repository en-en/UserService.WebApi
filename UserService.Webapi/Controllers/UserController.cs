using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UserService.IService;
using UserService.Model;

namespace UserService.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        private IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            this._userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("FindSingle")]
        public User FindSingle(int id)
        {
            return _userService.FindSingle(id);
        }
        [HttpGet]
        [Route("FindUsers")]
        public IEnumerable<User> FindUsers()
        {
            return this._userService.FindUsers().Select(u => new User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{ this._configuration["Service:IP"]}:{ this._configuration["Service:Port"]}",//多返回个信息
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }
    }
}
