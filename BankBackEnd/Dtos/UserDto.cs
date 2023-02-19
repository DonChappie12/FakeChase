using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankBackEnd.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}