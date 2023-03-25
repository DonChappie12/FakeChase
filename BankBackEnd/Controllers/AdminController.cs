using BankBackEnd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DBContext _context;

        public AdminController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            DBContext context
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("get-all-users")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _context.Users.ToList();
            return Ok(allUsers);
        }

        [HttpGet("get-one-user/{id}")]
        public async Task<IActionResult> GetOneUser([FromRoute]string id)
        {
            var getOne = await _context.Users.FindAsync(id);
            if(getOne == null)
                return BadRequest($"No user with Id: {id}");
                
            return Ok(getOne);
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