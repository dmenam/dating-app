using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO dto){

            if(await UserExist(dto.username)){
                return BadRequest("Username Taken");
            }
            
            using var hmac = new HMACSHA512(); //hash code variable, that after being called then its disposed due to the usage of 'using'
            
            //Create new object
            var user = new AppUser{
                UserName = dto.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)), //Gets the hash code of the password
                PasswordSalt = hmac.Key //Gets the HashSalt to scramble the hash code
            };

            _context.Users.Add(user); //Make entity framework track the change
            await _context.SaveChangesAsync(); //Upload to database

            return new UserDTO{
                username = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto) {
            
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == dto.username);

            if(user == null) return Unauthorized("Invalid username");

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserDTO{
                username = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExist(string username){
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); //Verifies if the username already exist in the db
        }
    }
}