using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;

namespace API.Entities
{
    public class AppUser : BaseUserDto
    {
        public Int32 Id { get; set; }

        public Byte[] PasswordHash {get; set; }

        public Byte[] PasswordSalt {get; set; }
    }
}