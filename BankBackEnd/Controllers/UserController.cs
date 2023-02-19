using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BankBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public void Get()
        {
            System.Console.WriteLine("Get");
        }

        [HttpPost]
        public void Post()
        {
            System.Console.WriteLine("post");
        }

        [HttpPut]
        public void Put()
        {
            System.Console.WriteLine("Put");
        }

        [HttpDelete]
        public void Delete()
        {
            System.Console.WriteLine("Delete");
        }
    }
}