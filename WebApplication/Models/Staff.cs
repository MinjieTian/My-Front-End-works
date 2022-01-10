using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Research { get; set; }


    }
}
