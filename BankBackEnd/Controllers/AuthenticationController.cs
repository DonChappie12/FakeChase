using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankBackEnd.Models;
using BankBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        // private readonly ILogger _logger;

        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            DBContext context,
            IConfiguration configuration
            // ILogger logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            // _logger = logger;
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

        [HttpPost("login-user")]
        public async Task<IActionResult> Login([FromBody]LoginVM loginUser)
        {
            if(!ModelState.IsValid)
                return BadRequest("Please provide all require fields");

            var userExist = await _userManager.FindByEmailAsync(loginUser.Email);
            if(userExist != null && await _userManager.CheckPasswordAsync(userExist, loginUser.Password))
            {
                var tokenValue = await GenerateJWTToken(userExist);
                return Ok(tokenValue);
            }
            return Unauthorized();
        }

        private async Task<AuthResultVM> GenerateJWTToken(User user)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSignInKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new AuthResultVM()
            {
                Token = jwtToken,
                ExpiresAt = token.ValidTo
            };

            return response;
        }
    }
}