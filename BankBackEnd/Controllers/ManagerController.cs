using BankBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager,Admin")]
    // ** Bottom code will tell authentication to authorize a user
    // ** if they are in both roles. EX: Manager AND Admin roles in
    // ** order to access this controller
    // [Authorize(Roles = "Admin")]
    public class ManagerController : ControllerBase
    {
        
        private readonly DBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ManagerController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            DBContext context
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("get-all-customers")]
        public IActionResult GetAllCustomers()
        {
            // Todo: Have this filter just customers as managers should not see admin accounts
            // var getCustomers = _context.Users.ToList();
            var getCustomers = _userManager.Users.ToList();
            var adminUsers = _roleManager.Roles.Single(r => r.Name == "Admin");

            // var userList = new List<IdentityUserRole>();
            return Ok();
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
    }
}