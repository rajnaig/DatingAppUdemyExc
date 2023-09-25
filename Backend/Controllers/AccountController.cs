using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Interfaces;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly AppDbContext _context;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(
            AppDbContext context,
            ITokenService tokenService,
            IMapper mapper)
        {
            _context = context;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {

            if (await UserExist(registerDto.UserName)) return BadRequest("Username is taken!");

            var user = mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x =>
                x.UserName == loginDto.UserName);

            if (user == null) return Unauthorized("This User name does not exist!");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var completedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < completedHash.Length; i++)
            {
                if (completedHash[i] != user.PasswordHash[i]) return Unauthorized("invalid Password!");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
            };
        }

        private async Task<bool> UserExist(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
