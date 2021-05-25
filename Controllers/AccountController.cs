using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using tablinumAPI.Models;
using tablinumAPI.Services;
using System.Security.Cryptography;
using System.Text;
using System.Security.Principal;
 
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
            var hash = "";
            User account = new User();
            account = _accountService.Get(user.UserLogin);
            if (account == null)
            {
                return BadRequest(new { errorText = "Нет такого пользователя!" });
            }
            using(var sha256 = SHA256.Create())  
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(user.Password + account.Salt));
                hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
            if (hash != account.Password) {
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

        public static bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        public static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidIssuer = AuthOptions.ISSUER,
                ValidAudience = AuthOptions.AUDIENCE,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey() // The same key as the one that generate the token
            };
        }
    }
}