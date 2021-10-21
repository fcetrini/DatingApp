using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class LoginUserDto : BaseUserDto
    {
        [Required]
        public String Password { get; set; }
    }
}