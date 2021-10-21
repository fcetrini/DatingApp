using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext Context, ITokenService TokenService)
        {
            this.tokenService = TokenService;
            this.context = Context;
        }

        [HttpPost(nameof(AccountController.Register))]
        public async Task<ActionResult<AppUser>> Register(RegisterUserDto RegisterDto)
        {
            if (await this.ensureUniqueness(RegisterDto.UserName) == false)
                return BadRequest("Username in use!");

            using (var hash = new HMACSHA512())
            {
                var user = new AppUser()
                {
                    UserName = RegisterDto.UserName.ToLower(),
                    PasswordHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(RegisterDto.Password)),
                    PasswordSalt = hash.Key
                };

                this.context.Users.Add(user);
                await this.context.SaveChangesAsync();
                return user;
            }
        }

        [HttpPost(nameof(AccountController.Login))]
        public async Task<ActionResult<LoggedUserDto>> Login(LoginUserDto LoginDto)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(u => u.UserName == LoginDto.UserName);

            if (user == null)
                return Unauthorized("Invalid username!");

            using (var hash = new HMACSHA512(user.PasswordSalt))
            {
                var calculatedHash = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(LoginDto.Password));
                
                if (user.PasswordHash.SequenceEqual(calculatedHash) == false)
                    return Unauthorized("Invalid password!");
            }

            return new LoggedUserDto
            {
                UserName = user.UserName,
                Token = this.tokenService.CreateToken(user)
            };
        }

        private async Task<Boolean> ensureUniqueness(String Username)
        {
            return await this.context.Users.AnyAsync(x => x.UserName == Username.ToLower()) == false;
        }
    }
}