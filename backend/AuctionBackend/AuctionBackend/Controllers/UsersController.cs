using AuctionBackend.Data;
using AuctionBackend.Models;
using AuctionBackend.Models.Entities;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace AuctionBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration configuration;

        public UsersController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("SignUp")]
        public IActionResult SignUp(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var objUser = dbContext.Users.FirstOrDefault(x => x.Username == userDto.Username);

            if(objUser == null)
            {
                var userEntity = new User
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Password = userDto.Password,
                };
                dbContext.Add(userEntity);
                dbContext.SaveChanges();
                return Ok(userEntity);
            }
            else
            {
                return BadRequest("User Already exists with the same username");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Username == loginDto.Username && x.Password == loginDto.Password);
            if (user != null)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("Email", user.Email.ToString()),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    configuration["Jwt:Issuer"],
                    configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                    );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenValue, User = user });
                //return Ok(user);
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(dbContext.Users.ToList());
        }

        [Authorize]
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser(Guid id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.UserId == id);
            if(user != null)
            {
                return Ok(user);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
