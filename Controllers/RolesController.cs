using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using tablinumAPI.Models;
using tablinumAPI.Services;

namespace tablinumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public ActionResult<List<Role>> Get() =>
            _roleService.Get();

        [HttpGet("{id:length(24)}", Name = "GetRole")]
        public ActionResult<Role> Get(string id)
        {
            var role = _roleService.Get(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        [HttpPost]
        public ActionResult<Role> Create(Role role)
        {
            _roleService.Create(role);

            return CreatedAtRoute("GetRole", new { id = role.Id.ToString() }, role);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Role roleIn)
        {
            var role = _roleService.Get(id);

            if (role == null)
            {
                return NotFound();
            }

            _roleService.Update(id, roleIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var role = _roleService.Get(id);

            if (role == null)
            {
                return NotFound();
            }

            _roleService.Remove(role.Id);

            return NoContent();
        }
    }
}