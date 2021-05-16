using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using tablinumAPI.Models;
using tablinumAPI.Services;

namespace tablinumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitiosController : ControllerBase
    {
        private readonly InitioService _initioService;

        public InitiosController(InitioService initioService)
        {
            _initioService = initioService;
        }

        [HttpGet]
        public ActionResult<List<Initio>> Get() =>
            _initioService.Get();

        [HttpGet("{id:length(24)}", Name = "GetInitio")]
        public ActionResult<Initio> Get(string id)
        {
            var initio = _initioService.Get(id);

            if (initio == null)
            {
                return NotFound();
            }

            return initio;
        }

        [HttpPost]
        public ActionResult<Initio> Create(Initio initio)
        {
            _initioService.Create(initio);

            return CreatedAtRoute("GetInitio", new { id = initio.Id.ToString() }, initio);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Initio initioIn)
        {
            var initio = _initioService.Get(id);

            if (initio == null)
            {
                return NotFound();
            }

            _initioService.Update(id, initioIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var initio = _initioService.Get(id);

            if (initio == null)
            {
                return NotFound();
            }

            _initioService.Remove(initio.Id);

            return NoContent();
        }
    }
}