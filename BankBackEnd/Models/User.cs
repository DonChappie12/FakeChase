using Microsoft.AspNetCore.Identity;

namespace BankBackEnd.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
    }
}