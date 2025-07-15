using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodosController : ControllerBase
    {
        private static List<TodoItem> todos = new List<TodoItem>
        {
            new TodoItem { Id = "c9d8eab0-4d34-410c-a432-5533cbf6047d", Value = "Learn java", Status = false },
            new TodoItem { Id = "4908861f-b013-43e4-bd3e-16e6542e4e94", Value = "Learn Python", Status = false },
            new TodoItem { Id = "c9223a41-0fa4-453d-8df2-11cf39d9f45d", Value = "Learn C#", Status = false },
        };

        // GET /todos
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAllTodos() => todos; // Returns all todos

        // POST /todos
        [HttpPost]
        public ActionResult<TodoItem> AddNewTodo([FromBody]TodoItem item) // Add new todo
        {
            if (item == null) return BadRequest();
            todos.Add(item);
            return CreatedAtAction(nameof(GetAllTodos), new { id = item.Id }, item);
        }

        // PATCH /todos/{id}
        [HttpPatch("{id}")]
        public ActionResult<TodoItem> EditTodo(string id, [FromBody] TodoItem patchItem) // Edit a todo by id (TodoItem.Value or TodoItem.Status or Both)
        {
            var targetTodo = todos.FirstOrDefault(item => item.Id == id);
            if(targetTodo == null)
            {
                return NotFound();
            }
            targetTodo.Value = patchItem.Value;
            targetTodo.Status = patchItem.Status;

            return Ok(targetTodo);
        }

        // DELETE /todo/{id}
        [HttpDelete("{id}")]
        public ActionResult<TodoItem> DeleteTodo(string id) // Remove a todo by id
        {
            if (id == null) return BadRequest();
            var targetTodo = todos.FirstOrDefault(item => item.Id == id);
            if(targetTodo == null)
            {
                return NotFound();
            }
            todos.Remove(targetTodo);
            return NoContent();
        }

    }
}
