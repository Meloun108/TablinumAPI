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
 
        [HttpPost("/token")]
        public IActionResult Token([FromBody]User user)
        {
            User account = new User();
            account = _accountService.Get(user.UserLogin);
            if (account == null)
            {
                return BadRequest(new { errorText = "Нет такого пользователя!" });
            }
            if (account.Password != user.Password) {
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

        [HttpPost("/register")]
        public IActionResult Register([FromBody]User user)
        {
            User account = new User();
            account = _accountService.Get(user.UserLogin);
            if (account == null)
            {
                User createUser = new User();
                createUser.UserLogin = user.UserLogin;
                createUser.Name = user.Name;
                createUser.GroupId = user.GroupId;
                createUser.RoleId = user.RoleId;
                createUser.Password = user.Password;
                account = _accountService.Create(createUser);
                var identity = GetIdentity(account);
                if (identity == null)
                {
                    return BadRequest(new { errorText = "Пользователь создан без учетной записи!" });
                }
                var now = DateTime.UtcNow;
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
            return BadRequest(new { errorText = "Такой пользователь уже существует!" });
        }
 
        private ClaimsIdentity GetIdentity(User account)
        {
            if (account != null)
            {
                if (account.RoleId == null) {
                    return null;
                }
                else {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, account.UserLogin),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Name),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, account.GroupId),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, account.RoleId),
                    };
                    ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                                           ClaimsIdentity.DefaultRoleClaimType);
                    return claimsIdentity;
                }
            }
            return null;
        }
    }
}