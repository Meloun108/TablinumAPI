using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using tablinumAPI.Models;
using tablinumAPI.Services;

namespace tablinumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupsController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public ActionResult<List<Group>> Get() =>
            _groupService.Get();

        [HttpGet("{id:length(24)}", Name = "GetGroup")]
        public ActionResult<Group> Get(string id)
        {
            var group = _groupService.Get(id);

            if (group == null)
            {
                return NotFound();
            }

            return group;
        }

        [HttpPost]
        public ActionResult<Group> Create(Group group)
        {
            _groupService.Create(group);

            return CreatedAtRoute("GetGroup", new { id = group.Id.ToString() }, group);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Group groupIn)
        {
            var group = _groupService.Get(id);

            if (group == null)
            {
                return NotFound();
            }

            _groupService.Update(id, groupIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var group = _groupService.Get(id);

            if (group == null)
            {
                return NotFound();
            }

            _groupService.Remove(group.Id);

            return NoContent();
        }
    }
}