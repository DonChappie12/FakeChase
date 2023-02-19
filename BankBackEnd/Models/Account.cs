namespace BankBackEnd.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountType { get; set; }
        public string SubAccountType { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string? ApprovedBy { get; set; }
    }
}