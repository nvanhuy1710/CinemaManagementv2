using Cinema.Model;
using Cinema.Module.Account.DTO;
using Cinema.Module.Account.Register;
using Cinema.Module.Account.Repository;
using Cinema.Module.Account.Service;
using Cinema.Module.User.DTO;
using Cinema.Module.User.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cinema.Controllers
{
    [Route("/api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IAccountService _accountService;

        private readonly IConfiguration _config;

        public LoginController(IUserService userService, IAccountService accountService, IConfiguration config)
        {
            _userService = userService;
            _accountService = accountService;
            _config = config;
        }

        [Route("/login")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginData loginData)
        {
            var account = _accountService.Login(loginData);
            if (account != null)
            {
                var token = GenerateToken(account.Email, account.RoleName);
                return Ok(token);
            }

            return NotFound();
        }

        [Route("/register")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([FromBody] RegisterData RegisterData)
        {
            var userDTO = _userService.Register(RegisterData);
            if (userDTO == null) return BadRequest("Email already exist");
            return Ok(userDTO);
        }

        [Route("/users")]
        [Authorize]
        [HttpGet]
        public ActionResult GetUser()
        {
            var userDTO = GetCurrentUser();

            if (userDTO == null)
            {
                return NotFound();
            }

            return Ok(userDTO);
        }

        private string GenerateToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Role, role),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private UserDTO GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return _userService.GetUserByEmail(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value.ToString());
            }
            return null;
        }
    }
}
