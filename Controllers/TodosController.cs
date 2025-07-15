using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("todos")]
    public class TodosController : ControllerBase
    {
        private readonly DapperService _dapperService;

        public TodosController(DapperService dapperService)
        {
            _dapperService = dapperService;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            try {
                var query = "SELECT * FROM Todos";
                using (var connection = _dapperService.CreateConnection())
                {
                    var todos = await connection.QueryAsync<TodoItem>(query);
                    return Ok(todos);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Db connection or query failed {ex.Message}");
            }

        } // Returns all todos

        // POST /todos
        [HttpPost]
        public async Task<IActionResult> AddNewTodo([FromBody] TodoItem item) // Add new todo
        {
            try
            {
                var query = "INSERT INTO Todos (Id, Value, Status) VALUES (@Id, @Value, @Status)";
                using (var connection = _dapperService.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { item.Id, item.Value, item.Status});
                    return CreatedAtAction(nameof(GetAllTodos), new { id = item.Id }, item);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Db connection or query failed {ex.Message}");
            }
            
            
        }

        // PATCH /todos/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditTodo(string id, [FromBody] TodoItem patchItem) // Edit a todo by id (TodoItem.Value or TodoItem.Status or Both)
        {
            var query = @"UPDATE Todos 
                        SET Value = @Value, Status = @Status 
                        WHERE Id = @Id";
            try
            {
                using (var connection = _dapperService.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { patchItem.Value, patchItem.Status, Id = id });
                    return Ok(new { id });
                }
            }
            catch(Exception ex)
            {
                return BadRequest($"Db connection or query failed {ex.Message}");
            }
        }

        // DELETE /todo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(string id) // Remove a todo by id
        {
            var query = "DELETE FROM Todos WHERE Id = @Id";
            try
            {
                using (var connection = _dapperService.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new { Id = id });
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Db connection or query failed {ex.Message}");
            }
        }

    }
}

//SQL TABLE STRUCTURE 

//CREATE TABLE Todos(
//	Id		NVARCHAR(80) PRIMARY KEY,
//    Value	NVARCHAR(150) NOT NULL,
//    Status	BIT DEFAULT 0 NOT NULL
//) 
