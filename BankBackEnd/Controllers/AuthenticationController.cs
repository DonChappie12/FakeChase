using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BankBackEnd.Models;
using BankBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly TokenValidationParameters _tokenValidationParameters;
        // private readonly ILogger _logger;

        public AuthenticationController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            DBContext context,
            IConfiguration configuration,
            TokenValidationParameters tokenValidationParameters
            // ILogger logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
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

            // ** Every new customer will be associated to customer role
            var result = await _userManager.CreateAsync(newUser, registerUser.Password);
            if(result.Succeeded) 
            {
                await _userManager.AddToRoleAsync(newUser, "Customer");
                return Ok(newUser);
            }

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
                var tokenValue = await GenerateJWTToken(userExist, null);
                return Ok(tokenValue);
            }
            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody]TokenRequestVM tokenRequest)
        {
            if(!ModelState.IsValid)
                return BadRequest("Please provide all require fields");

            var result = await VerifyAndGenerateTokenAsync(tokenRequest);
            return Ok(result);
        }

        private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM token)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token.RefreshToken);
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(token.Token, _tokenValidationParameters, out var validatedToken);

                    return await GenerateJWTToken(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException )
            {
                if(storedToken.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateJWTToken(dbUser, storedToken);
                }
                else
                {
                    return await GenerateJWTToken(dbUser, null);
                }
            }
        }

        private async Task<AuthResultVM> GenerateJWTToken(User user, RefreshToken rToken)
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

            if(rToken != null)
            {
                var rTokenResponse = new AuthResultVM()
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };

                return rTokenResponse;
            }

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                // ? May have to change this since this is a banking app so token shouldn't be available all the time
                DateExpire = DateTime.UtcNow.AddMonths(1),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultVM()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };

            return response;
        }
    }
}