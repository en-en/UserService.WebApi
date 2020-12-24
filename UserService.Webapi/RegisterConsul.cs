using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MicroService.Consul
{
    public static class RegisterConsul
    {
        //public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime,
        //    ServiceEntity entity)
        //{
        //    //请求注册的consul地址
        //    var consulClient=new ConsulClient(x=>x.Address=new Uri($"http://{entity.ConsulIP}:{entity.ConsulPort}"));

        //    var httpCheck=new AgentServiceCheck()
        //    {
        //        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
        //        Interval = TimeSpan.FromSeconds(10),
        //        HTTP = $"Http://{entity.IP}:{entity.Port}/api/health/index",
        //        Timeout = TimeSpan.FromSeconds(5)

        //    };
        //    //注册到consul
        //    var reagistration = new AgentServiceRegistration()
        //    {
        //        Checks = new[] {httpCheck},
        //        ID = Guid.NewGuid().ToString(),
        //        Name = entity.ServiceName,
        //        Address = entity.IP,
        //        Port = entity.Port,
        //        Tags = new[] {$"urlprefix-/{entity.ServiceName}"}
        //    };
        //    //服务启动时注册，内部实际由consul api注册 httpClient发起
        //    consulClient.Agent.ServiceRegister(reagistration).Wait();
          
        //    lifetime.ApplicationStopped.Register(() =>
        //    {
        //        consulClient.Agent.ServiceDeregister(reagistration.ID).Wait();
        //    });
        //    return app;
        //}

        public static IApplicationBuilder UseConul(this IApplicationBuilder app, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            Console.WriteLine("consul注册");
            try
            {
              var client = new ConsulClient(options =>
                {
                    options.Address = new Uri($"http://{configuration["Consul:IP"]}:{configuration["Consul:Port"]}"); // Consul客户端地址
                });

                var registration = new AgentServiceRegistration
                {
                    ID = Guid.NewGuid().ToString(), // 唯一Id
                    Name = configuration["Service:Name"], // 服务名
                    Address = configuration["Service:IP"], // 服务绑定IP
                    Port = Convert.ToInt32(configuration["Service:Port"]), // 服务绑定端口
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), // 服务启动多久后注册
                        Interval = TimeSpan.FromSeconds(10), // 健康检查时间间隔
                        HTTP = $"http://{configuration["Service:IP"]}:{configuration["Service:Port"]}{configuration["Consul:HealthCheck"]}", // 健康检查地址
                        Timeout = TimeSpan.FromSeconds(5) // 超时时间
                    }
                };

                // 注册服务
                client.Agent.ServiceRegister(registration).Wait();

                // 应用程序终止时，取消服务注册
                lifetime.ApplicationStopping.Register(() =>
                {
                    client.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
            catch (Exception ex)
            {

                Console.WriteLine($"consul注册失败，{ex.Message}");
            }
            Console.WriteLine("consul注册成功");
            return app;
        }
    }

}
