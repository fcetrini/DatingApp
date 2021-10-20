using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public Int32 Id { get; set; }
        public String UserName { get; set; }

        public Byte[] PasswordHash {get; set; }

        public Byte[] PasswordSalt {get; set; }
    }
}