
using Microsoft.AspNetCore.Identity;
using sendITAPI.Models;

namespace sendITAPI.Services
{
    public class BcryptPasswordHasher : IPasswordHasher<User> 
    {
        public string HashPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(SaltPassword(user, password), 10);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            if (BCrypt.Net.BCrypt.Verify(SaltPassword(user, providedPassword), hashedPassword))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }

        private string SaltPassword(User user, string password)
        {
            return password;
        }
    }

}