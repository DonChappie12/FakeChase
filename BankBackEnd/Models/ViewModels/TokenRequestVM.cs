using System.ComponentModel.DataAnnotations;

namespace BankBackEnd.Models.ViewModels
{
    public class TokenRequestVM
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}