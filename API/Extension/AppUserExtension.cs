using API.DTOs;
using API.Entities;
using API.Interface;

namespace API.Extension
{
    public static class AppUserExtension
    {
        public static async Task<UserDto> ToDto(this AppUser user, ITokenService tokenService)
        {
            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                ImageUrl= user.ImageUrl,
                Token = await tokenService.CreateToken(user)
            };
        }
    }
}
