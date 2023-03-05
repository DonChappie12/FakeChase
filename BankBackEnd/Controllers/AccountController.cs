using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    // [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        // private readonly IMapper _mapper;
        public AccountController()
        {
            // _mapper = mapper;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok("This is the AccountContoller");
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