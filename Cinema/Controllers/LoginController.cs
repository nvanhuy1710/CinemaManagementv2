using Cinema.Helper;
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
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IAccountService _accountService;

        private readonly TokenProvider _tokenProvider;

        public LoginController(IUserService userService, IAccountService accountService, TokenProvider tokenProvider)
        {
            _userService = userService;
            _accountService = accountService;
            _tokenProvider = tokenProvider;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginData loginData)
        {
            try
            {
                loginData.Password = HashPassword.HashByPBKDF2(loginData.Password);
                var account = _accountService.Login(loginData);
                var token = _tokenProvider.GenerateToken(account.Email, account.RoleName);
                return Ok(new { accessToken = token });
            } catch (InvalidOperationException)
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterData RegisterData)
        {
            RegisterData.Password = HashPassword.HashByPBKDF2(RegisterData.Password);
            var userDTO = _userService.Register(RegisterData);
            if (userDTO == null) return BadRequest("Email already exist");
            return Ok();
        }

        [Authorize]
        [HttpGet("users")]
        public ActionResult GetUser()
        {
            var userDTO = GetCurrentUser();

            if (userDTO == null)
            {
                return NotFound();
            }
            return Ok(userDTO);
        }

        [Authorize]
        [HttpPut("users")]
        public ActionResult UpdateUser([FromBody] UserDTO userDTO)
        {
            var newUserDTO = _userService.UpdateUser(userDTO);
            return Ok(newUserDTO);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("admins/add-staff")]
        public IActionResult AddStaff([FromBody] RegisterData RegisterData)
        {
            RegisterData.Password = HashPassword.HashByPBKDF2(RegisterData.Password);
            var userDTO = _userService.Register(RegisterData, true);
            if (userDTO == null) return BadRequest("Email already exist");
            return Ok();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("admins/get-staff")]
        public IActionResult GetStaff()
        {
            return Ok(_userService.GetStaff());
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("admins/delete-staff/{id}")]
        public IActionResult DeleteStaff(int id)
        {
            _userService.DeleteUser(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut("users/change-password")]
        public IActionResult ChangePassword([FromBody] PasswordChangeForm form)
        {
            try
            {
                form.oldPassword = HashPassword.HashByPBKDF2(form.oldPassword);
                form.newPassword = HashPassword.HashByPBKDF2(form.newPassword);
                _userService.ChangePassword(form, GetCurrentUser().Id);
                return NoContent();
            }
            catch(InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("users/forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                _userService.ForgotPassword(email);
                return Ok("Sent email");
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
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
