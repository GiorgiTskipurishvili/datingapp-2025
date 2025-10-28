using API.Data;
using API.DTOs;
using API.Entities;
using API.Extension;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(AppDbContext context, ITokenService tokenService) : BaseApiController
    {
        [HttpPost("register")] //localhost:5000/api/account/register    //string email, string displayName, string password
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await EmailExists(registerDto.Email)) return BadRequest("Email Taken");

            using var hmac = new HMACSHA512(); //generates a random key

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key,
                Member = new Member
                {
                    DisplayName = registerDto.DisplayName,
                    Gender = registerDto.Gender,
                    City = registerDto.City,
                    Country = registerDto.Country,
                    DateOfBirth = registerDto.DateOfBirth
                }
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.ToDto(tokenService);    
        }



        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(x=> x.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalid email address");
            
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (var i =0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            //return new UserDto
            //{
            //    Id = user.Id,
            //    DisplayName = user.DisplayName,
            //    Email = user.Email,
            //    Token = tokenService.CreateToken(user)
            //};    
            return user.ToDto(tokenService);


        }


        private async Task<bool> EmailExists(string email)
        {
            //return await context.Users.AnyAsync(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
            return await context.Users.AnyAsync(x => x.Email.ToLower() == email.ToLower());

        }

    }
}
