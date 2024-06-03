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
        [SwaggerResponse(HttpStatusCode.OK, typeof(List<TodoItemDTO>), Description = "Returns list of TodoItems")]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetAllTodoItems()
        {
            return await context.TodoItems.Select(x => new TodoItemDTO(x)).ToListAsync();
        }

        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(TodoItemDTO), Description = "Returns a TodoItem")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundResult), Description = "If not found")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            return await context.TodoItems.FindAsync(id) is TodoItem todoItem
                ? new TodoItemDTO(todoItem)
                : NotFound();
        }

        // POST: api/TodoItems
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, typeof(TodoItemDTO), Description = "TodoItem Created")]
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
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(NoContentResult), Description = "Returns no content")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundResult), Description = "If not found")]
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
