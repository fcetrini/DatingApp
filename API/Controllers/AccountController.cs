using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;

        public AccountController(DataContext context)
        {
            this.context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await this.ensureUniqueness(registerDto.Username) == false)
                return BadRequest("Username in use!");

            using (var hash = new HMACSHA512())
            {
                var user = new AppUser()
                {
                    UserName = registerDto.Username.ToLower(),
                    PasswordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hash.Key
                };

                this.context.Users.Add(user);
                await this.context.SaveChangesAsync();
                return user;
            }
        }

        private async Task<Boolean> ensureUniqueness(String Username)
        {
            return await this.context.Users.AnyAsync(x => x.UserName == Username.ToLower()) == false;
        }
    }
}