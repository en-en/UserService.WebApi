using System.Collections.Generic;
using System.Linq;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
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

        //public void SendSms()
        //{
        //    //在控制台完成短信签名与短信模板的申请，获得调用接口必备的参数。
        //    string product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
        //    string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
        //    string accessKeyId = "";//你的accessKeyId，参考本文档步骤2
        //    string accessKeySecret = "";//你的accessKeySecret，参考本文档步骤2
        //    IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
        //    //IAcsClient client = new DefaultAcsClient(profile);
        //    // SingleSendSmsRequest request = new SingleSendSmsRequest();
        //    //初始化ascClient,暂时不支持多region（请勿修改）
        //    profile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
        //    IAcsClient acsClient = new DefaultAcsClient(profile);
        //    SendSmsRequest request = new SendSmsRequest();
        //    try
        //    {
        //        //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
        //        request.PhoneNumbers = "136";
        //        //必填:短信签名-可在短信控制台中找到
        //        request.SignName = "xxxxxxxx";
        //        //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
        //        request.TemplateCode = "SMS_00000001";
        //        //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
        //        request.TemplateParam = "{\"name\":\"Tom\",\"code\":\"123\"}";
        //        //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
        //        request.OutId = "yourOutId";
        //        //请求失败这里会抛ClientException异常
        //        SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);

        //        System.Console.WriteLine(sendSmsResponse.Message);


        //    }
        //    catch (ServerException e)
        //    {
        //        System.Console.WriteLine("Hello World!");
        //    }
        //    catch (ClientException e)
        //    {
        //        System.Console.WriteLine("Hello World!");
        //    }
        //}


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
