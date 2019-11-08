using System;
using System.Collections.Generic;
using System.Linq;
using sendITAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace sendITAPI.Services
{
    

    public interface IUserService
    {
        User Authenticate(User user, User foundUser);
        List<User> GetAll();
        User CreateUser(User user);

        User FindUserByEmail(User user);

        AuthPayload GenerateToken(User user);
    }
    
  
    public class UserService : IUserService
    {
        private readonly UserParcelContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly Startup.AppSettings _appSettings;

        public UserService(UserParcelContext context, IPasswordHasher<User> passwordHasher, IOptions<Startup.AppSettings> appSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings.Value;
        }
        

        public User Authenticate(User user, User foundUser)
        {
            var verifyResult = _passwordHasher.VerifyHashedPassword(foundUser, foundUser.Password, user.Password);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            if (verifyResult == PasswordVerificationResult.Success)
            {
                foundUser.Password = null;
                return foundUser;
            }

            return null;
        }

        public User FindUserByEmail(User user)
        {
            try
            {
                return _context.Users.Single(u => u.Email == user.Email);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        

        public List<User> GetAll()
        {
            var users = _context.Users;
            
            return users.ToList();
        }

        public User CreateUser(User user)
        {
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();
            user.Password = null;
            return user;
        }

        public AuthPayload GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                    }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var finalToken = tokenHandler.WriteToken(token);
            
            var authPayload = new AuthPayload
            {
                Email = user.Email,
                Token = finalToken,
                Id = user.Id
            };
            return authPayload;
        }
    }

    public class AuthPayload
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public long Id { get; set; }
        
        public string Message { get; set; }
        
    }

}