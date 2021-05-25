using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using tablinumAPI.Models;
using tablinumAPI.Services;
 
namespace tablinumAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
    private readonly AccountService _userService;

        public UserController(AccountService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get([FromHeader]string authorization) {
            AccountController.ValidateToken(authorization);
            var users = _userService.Get();
            return users;
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> Get([FromHeader]string authorization, string id)
        {
            AccountController.ValidateToken(authorization);
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("{login}")]
        public ActionResult<User> GetLogin([FromHeader]string authorization, string login)
        {
            AccountController.ValidateToken(authorization);
            var user = _userService.Get(login);
            user.Password = null;
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public ActionResult<User> Create([FromHeader]string authorization, User user)
        {
            AccountController.ValidateToken(authorization);
            _userService.Create(user);
            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update([FromHeader]string authorization, string id, User userIn)
        {
            AccountController.ValidateToken(authorization);
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Update(id, userIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromHeader]string authorization, string id)
        {
            AccountController.ValidateToken(authorization);
            var user = _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            _userService.Remove(user.Id);
            return NoContent();
        }
    }
}