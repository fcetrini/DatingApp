using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class LoggedUserDto : BaseUserDto
    {
        public String Token { get; set;}
    }
}