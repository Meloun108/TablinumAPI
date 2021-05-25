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
        public ActionResult<List<Role>> Get([FromHeader]string authorization) {
            AccountController.ValidateToken(authorization);
            var roles = _roleService.Get();
            return roles;
        }

        [HttpGet("{id:length(24)}", Name = "GetRole")]
        public ActionResult<Role> Get([FromHeader]string authorization, string id)
        {
            AccountController.ValidateToken(authorization);
            var role = _roleService.Get(id);
            if (role == null)
            {
                return NotFound();
            }
            return role;
        }

        [HttpPost]
        public ActionResult<Role> Create([FromHeader]string authorization, Role role)
        {
            AccountController.ValidateToken(authorization);
            _roleService.Create(role);
            return CreatedAtRoute("GetRole", new { id = role.Id.ToString() }, role);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update([FromHeader]string authorization, string id, Role roleIn)
        {
            AccountController.ValidateToken(authorization);
            var role = _roleService.Get(id);
            if (role == null)
            {
                return NotFound();
            }
            _roleService.Update(id, roleIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromHeader]string authorization, string id)
        {
            AccountController.ValidateToken(authorization);
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