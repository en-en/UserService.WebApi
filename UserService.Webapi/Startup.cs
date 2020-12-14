﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using UserService.IService;
using UserService.Webapi.Controllers;

namespace UserService.Webapi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvcCore()
                           .AddAuthorization()//添加身份验证服务
                           .AddJsonFormatters();
            //AddAuthentication将身份验证服务添加到DI并配置"Bearer"为默认方案
            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = "http://" + Configuration["IdentityServer4:IP"] + ":" + Configuration["IdentityServer4:Port"] + "/";//identifyServer服务地址
                        options.RequireHttpsMetadata = false;//是否使用https

                        options.ApiName = "UserServiceApi";//进行身份验证的API资源的名称
                    });
          //  Configuration["IdentityServer4:IP"] + ":" + Configuration["IdentityServer4:Port"]
            //Configuration["IdentityServer4:IP"] + ":" + Configuration["IdentityServer4:Port"]
            services.AddTransient<IUserService, UserService.Service.UserService>();

            //注册swagger服务
            services.AddSwaggerGen((s) =>
            {

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
{
            { new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference()
            {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
            }
            }, Array.Empty<string>() }
});

                //唯一标识文档的URI友好名称
                s.SwaggerDoc("swaggerName", new OpenApiInfo()
                {
                    Title = "swagger集成配置测试",//（必填）申请的标题。
                    Version = "5.3.1",//（必填）版本号(这里直接写的是Swashbuckle.AspNetCore包的版本号,(有写 v1 的))
                    Description = "用户描述信息",//对应用程序的简短描述。
                    Contact = new OpenApiContact()//公开API的联系信息
                    {
                        Email = "123456789@qq.com",
                        Name = "张三",
                        Extensions = null,
                        Url = null
                    },
                    License = new OpenApiLicense()//公开API的许可信息
                    {
                        Name = "张三",
                        Extensions = null,
                        Url = null
                    }
                });

                //添加中文注释 
                //拼接生成的XML文件路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //HomeController为当前程序集下的一个类（可自定义一个当前应用程序集下的一个类）[用于获取程序集名称]
                var commentsFileName = typeof(HealthController).Assembly.GetName().Name + ".xml";
                var xmlPath = Path.Combine(basePath, commentsFileName);
                s.IncludeXmlComments(xmlPath);
                s.DocInclusionPredicate((docName, description) => true);

            });

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            //app.UseIdentityServer();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI((s) =>
            {
                //注意：/swagger/唯一标识文档的URI友好名称/swagger.josn   
                s.SwaggerEndpoint("/swagger/swaggerName/swagger.json", "UserService");
                s.RoutePrefix = "";
            });

            ServiceEntity serviceEntity = new ServiceEntity
            {
                IP = Configuration["Service:IP"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
                ConsulIP = Configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])

            };
            Console.WriteLine($"consul开始注册{JsonConvert.SerializeObject(serviceEntity)}");
            app.RegisterConsul(lifetime, serviceEntity);
        }
    }
}
