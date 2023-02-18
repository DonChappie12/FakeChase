using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace BankBackEnd.Models
{
    public class DBContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        protected readonly IConfiguration Configuration;
        public DBContext(IConfiguration configuration, DbContextOptions<DBContext> options) : base(options)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = Configuration.GetConnectionString("DBConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
    }
}