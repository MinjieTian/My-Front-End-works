using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Dto
{
    public class CommentInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Comment{ get; set; }


    }
}
