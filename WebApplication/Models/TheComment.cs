using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WebApplication.Models
{
    public class TheComment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Time { get; set; }


        [Required]
        public string Comment { get; set; }

        
        public string Name { get; set; }


        [Required]
        public string IP{get;set; }



    }
}
