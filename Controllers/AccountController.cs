using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using tablinumAPI.Models;
using tablinumAPI.Services;
 
namespace tablinumAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }
        /*private List<User> users = new List<User>
        {
            new User {Login="admin@gmail.com", Password="12345", Group="1T", Name="Administrator", Role = "admin" },
            new User { Login="qwerty@gmail.com", Password="55555", Group="Secretary", Name="V", Role = "user" }
        };*/
 
        [HttpPost("/token")]
        public IActionResult Token(string username, string password)
        {
            User account = new User();
            account = _accountService.Get(username);
            //var account = _accountService.Get(username);

            if (account == null)
            {
                return BadRequest(new { errorText = "Нет такого пользователя!" });
            }
            if (account.Password != password) {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
            var identity = GetIdentity(account);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }
 
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Json(response);
        }
 
        private ClaimsIdentity GetIdentity(User account)
        {
            if (account != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.UserLogin),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Group),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role),
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}