using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSwag.Annotations;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController(TodoContext context) : ControllerBase
    {
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllTodoItems()
        {
            return await context.TodoItems.Select(x => new TodoItemDTO(x)).ToListAsync();
        }

        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            return await context.TodoItems.FindAsync(id) is TodoItem todoItem
                ? new TodoItemDTO(todoItem)
                : NotFound();
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem(todoItemDTO);

            context.TodoItems.Add(todoItem);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostTodoItem), new { id = todoItem.Id }, new TodoItemDTO(todoItem));
        }

        // PUT: api/TodoItems/{id}
        [HttpPut("{id}")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(NoContentResult), Description = "Returns no content")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestResult), Description = "If request is bad")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundResult), Description = "If not found")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            if (await context.TodoItems.FindAsync(id) is TodoItem todoItem)
            {
                todoItem.Name = todoItemDTO.Name;
                todoItem.IsComplete = todoItemDTO.Done;
                await context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        // DELETE: api/TodoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (await context.TodoItems.FindAsync(id) is TodoItem todoItem)
            {
                context.TodoItems.Remove(todoItem);
                await context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }
}
