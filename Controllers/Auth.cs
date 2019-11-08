using System;
using Microsoft.AspNetCore.Mvc;
using sendITAPI.Models;
using sendITAPI.Services;
using System.Collections.Generic;

namespace sendITAPI.Controllers
{
    
      [ApiController]
      [Route("[controller]")]
      public class Auth : ControllerBase
    {
        private readonly IUserService _userService;


        public Auth(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>>GetAll()
        {
            var users = _userService.GetAll();

            if (users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users);
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(User user)
        {
            var foundUser = _userService.FindUserByEmail(user);

            if (foundUser == null)
            {
                return StatusCode(403);
            }

            var authUser = _userService.Authenticate(user, foundUser);

            if (authUser == null)
            {
                return StatusCode(403);
            }
            
            
            var token =  _userService.GenerateToken(authUser);

            token.Message = "You are successfully logged in";

            return Ok(token);


        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User user)
        {
            var foundUser = _userService.FindUserByEmail(user);

            if (foundUser == null)
            {
                var newUser = _userService.CreateUser(user);
                
               var token = _userService.GenerateToken(newUser);

               token.Message = "An account has been created for you successfully";
               
               return Ok(token);

            }

            return Conflict("Email is not available");

        }
    }
}