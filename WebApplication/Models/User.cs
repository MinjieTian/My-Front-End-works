using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Address { get; set; }
    }
}
