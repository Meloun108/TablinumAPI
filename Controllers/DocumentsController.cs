using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tablinumAPI.Models;
using tablinumAPI.Services;
using System.Web.Http.Cors;

namespace tablinumAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentService _documentService;

        public DocumentsController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public ActionResult<List<Document>> Get([FromHeader]string authorization) {
            AccountController.ValidateToken(authorization);
            var docs = _documentService.Get();
            return docs;
        }

        [HttpGet("{id:length(24)}", Name = "GetDocument")]
        public ActionResult<Document> Get([FromHeader]string authorization, string id)
        {
            AccountController.ValidateToken(authorization);
            var document = _documentService.Get(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        [HttpGet("group/{group}")]
        public ActionResult<List<Document>> GetGroup([FromHeader]string authorization, string group)
        {
            AccountController.ValidateToken(authorization);
            var docs = _documentService.GetUserDoc(group);
            return docs;
        }

        [HttpPost]
        public ActionResult<Document> Create([FromHeader]string authorization, Document document)
        {
            AccountController.ValidateToken(authorization);
            _documentService.Create(document);
            return CreatedAtRoute("GetDocument", new { id = document.Id.ToString() }, document);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update([FromHeader]string authorization, string id, Document documentIn)
        {
            AccountController.ValidateToken(authorization);
            var document = _documentService.Get(id);
            if (document == null)
            {
                return NotFound();
            }
            _documentService.Update(id, documentIn);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete([FromHeader]string authorization, string id)
        {
            AccountController.ValidateToken(authorization);
            var document = _documentService.Get(id);
            if (document == null)
            {
                return NotFound();
            }
            _documentService.Remove(document.Id);
            return NoContent();
        }
    }
}

/*namespace tablinumAPI.Controllers
{
    [Route("api/TablinumItems")]
    [ApiController]
    public class TablinumItemsController : ControllerBase
    {
        private readonly TablinumContext _context;

        public TablinumItemsController(TablinumContext context)
        {
            _context = context;
        }

        // GET: api/TablinumItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TablinumItem>>> GetTablinumItems()
        {
            return await _context.TablinumItems.ToListAsync();
        }

        // GET: api/TablinumItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TablinumItemDTO>> GetTablinumItem(long id)
        {
            var tablinumItem = await _context.TablinumItems.FindAsync(id);

            if (tablinumItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(tablinumItem);
        }

        // PUT: api/TablinumItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTablinumItem(long id, TablinumItemDTO tablinumItemDTO)
        {
            if (id != tablinumItemDTO.Id)
            {
                return BadRequest();
            }

            var tablinumItem = await _context.TablinumItems.FindAsync(id);
            if (tablinumItem == null) {
                return NotFound();
            }

            tablinumItem.Name = tablinumItemDTO.Name;
            tablinumItem.IsComplete = tablinumItemDTO.IsComplete;

            //_context.Entry(tablinumItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TablinumItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TablinumItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TablinumItemDTO>> PostTablinumItem(TablinumItemDTO tablinumItemDTO)
        {
            var tablinumItem = new TablinumItem
            {
                IsComplete = tablinumItemDTO.IsComplete,
                Name = tablinumItemDTO.Name
            };

            _context.TablinumItems.Add(tablinumItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTablinumItem", new { id = tablinumItem.Id }, tablinumItem);
            return CreatedAtAction(nameof(GetTablinumItem), new { id = tablinumItem.Id }, ItemToDTO(tablinumItem));
        }

        // DELETE: api/TablinumItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTablinumItem(long id)
        {
            var tablinumItem = await _context.TablinumItems.FindAsync(id);
            if (tablinumItem == null)
            {
                return NotFound();
            }

            _context.TablinumItems.Remove(tablinumItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TablinumItemExists(long id)
        {
            return _context.TablinumItems.Any(e => e.Id == id);
        }

        private static TablinumItemDTO ItemToDTO(TablinumItem tablinumItem) => new TablinumItemDTO
        {
            Id = tablinumItem.Id,
            Name = tablinumItem.Name,
            IsComplete = tablinumItem.IsComplete
        };
    }
}*/
