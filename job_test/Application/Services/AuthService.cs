using job_test.Application.DTOs.Auth;
using job_test.Application.Exceptions;
using job_test.Application.Interfaces;
using job_test.Domain.Models;
using job_test.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace job_test.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        private readonly IJwtService _jwtService;

        public AuthService(ApplicationDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            var exists = await _context.Users.AnyAsync(x => x.Email == dto.Email);

            if (exists)
            {
                throw new BadRequestException("Email already exists");
            }

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return "User registered successfully";
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
            {
                return null;
            }

            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!validPassword)
            {
                return null;
            }

            var token = _jwtService.GenerateToken( user.Id, user.UserName,  user.Email  );

            return new AuthResponseDto
            {
                Token = token,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
