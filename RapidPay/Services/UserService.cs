using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using RapidPay.Data;
using System.Threading.Tasks;

namespace RapidPay.services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        // Registration
        public async Task RegisterUser(string username, string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt,
                CreatedAt = DateTime.Now,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Login
        public async Task<User> ValidateUser(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }

}