using BankBackEnd.Models;
using BankBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DBContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            DBContext context,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody]RegisterVM registerUser)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var userExist = await _userManager.FindByEmailAsync(registerUser.EmailAddress);
            if(userExist != null)
                return BadRequest($"User {registerUser.EmailAddress} already exists");

            User newUser = new User()
            {
                FirstName = registerUser.FirstName,
                MiddleName = registerUser.MiddleName,
                LastName = registerUser.LastName,
                Email = registerUser.EmailAddress,
                UserName = registerUser.UserName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, registerUser.Password);
            if(result.Succeeded) return Ok(newUser);

            return BadRequest("User could not be created");
        }
    }
}