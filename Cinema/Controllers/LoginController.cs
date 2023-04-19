﻿using Cinema.Helper;
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

        private readonly TokenProvider _tokenProvider;

        public LoginController(IUserService userService, IAccountService accountService, TokenProvider tokenProvider)
        {
            _userService = userService;
            _accountService = accountService;
            _tokenProvider = tokenProvider;
        }

        [Route("/login")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginData loginData)
        {
            /*loginData.Password = HashPassword.HashByPBKDF2(loginData.Password);*/
            var account = _accountService.Login(loginData);
            if (account != null)
            {
                var token = _tokenProvider.GenerateToken(account.Email, account.RoleName);
                return Ok(new { accessToken = token });
            }
            return NotFound();
        }

        [Route("/register")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([FromBody] RegisterData RegisterData)
        {
            RegisterData.Password = HashPassword.HashByPBKDF2(RegisterData.Password);
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

        [Route("/users")]
        [Authorize]
        [HttpPut]
        public ActionResult UpdateUser([FromBody] UserDTO userDTO)
        {
            var newUserDTO = _userService.UpdateUser(userDTO);
            return Ok(newUserDTO);

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
