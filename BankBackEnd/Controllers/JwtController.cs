using Microsoft.AspNetCore.Mvc;

namespace BankBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JwtController : ControllerBase
    {
        public void CreateToken()
        {
            System.Console.WriteLine("Creates Token");
        }

        public void ValidateToken()
        {
            System.Console.WriteLine("Validate Token");
        }

        public void UpdateToken()
        {
            System.Console.WriteLine("Update Token");
        }
    }
}