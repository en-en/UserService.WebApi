﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace UserService.Webapi.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private IConfiguration _configuration;

        public HealthController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var ip =   HttpContext.Connection.LocalIpAddress.MapToIPv4()?.ToString();
            Console.WriteLine(ip);
            Console.WriteLine($"This is HealthController  {this._configuration["Service:Port"]} Invoke");
            return Ok();
        }
    }
}
