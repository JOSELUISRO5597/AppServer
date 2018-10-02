using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppServer.Models
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        
        public string Password { get; set; }

        public string Image { get; set; }

        public string Token { get; set; }
    }
}
