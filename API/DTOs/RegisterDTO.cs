using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required] //Makes the field required
        public string username {get; set;}
        [Required]
        public string password {get;set;}
    }
}