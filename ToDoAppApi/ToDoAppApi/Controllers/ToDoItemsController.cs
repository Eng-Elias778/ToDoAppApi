using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;
using ToDoAppApi.Data;
using ToDoAppApi.Models;

namespace ToDoAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ToDoItemsController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAll()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        [HttpGet]
        [Route("GetById/{Id}")]
        public async Task<ActionResult<ToDoItem>> GetById(int Id)
        {
            var data = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == Id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        [HttpPost]
        [Route("AddItem")]
        public async Task<ActionResult<ToDoItem>> Add(ToDoItem Item)
        {
            _context.ToDoItems.Add(Item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = Item.Id }, Item);
        }

        [HttpPut]
        [Route("UpdateItem")]
        public async Task<IActionResult> Update(ToDoItem Item)
        {
            var data = await _context.ToDoItems.FirstOrDefaultAsync(x => x.Id == Item.Id);
            if (data == null)
                return Ok("Item Not Fount");

            data.Title = Item.Title;
            data.IsCompleted = Item.IsCompleted;

            await _context.SaveChangesAsync();

            return Ok("Item Updated Successfully");
            
        }

        [HttpDelete]
        [Route("DeleteItem/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var data = _context.ToDoItems.FirstOrDefault(x=>x.Id == Id);

            if(data == null)
                return NotFound();

            _context.ToDoItems.Remove(data);
            await _context.SaveChangesAsync();
            return Ok("Delete Success");
        }
    }
}
