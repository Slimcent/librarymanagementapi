using Library.Entities.Entities;
using Library.Entities.Entities.DTO;
using Library.Entities.Helpers;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationManager _authManager;

        public AuthenticationController(UserManager<User> userManager, IConfiguration configuration, IAuthenticationManager authManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _authManager = authManager;
        }

        [HttpPost]
        [Route("Register")]


        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {

            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Username already exists" });


            User newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed Try Again" });

            }
            return Ok(new Response { Status = "Success", Message = "User Created Successfully" });

        }


        [HttpPost("login")]

        public async Task<IActionResult> Authenticate([FromBody] LoginModel loginModel)
        {
            if (!await _authManager.ValidateUser(loginModel))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Failed Try Again" });
            }
            return Ok(new { Token = await _authManager.CreateToken() });
        }

    }
}




