using Application.DTOs;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace todolistapp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IHubContext<TaskHub> _taskHub;

        public TasksController(ITaskService taskService, IHubContext<TaskHub> taskHub)
        {
            _taskService = taskService;
            _taskHub = taskHub;
        }        

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? userId, [FromQuery] TaskProgress? status, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _taskService.GetTasksAsync(userId, status, from, to, search, page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (dto.DueDate <= DateTime.UtcNow) return BadRequest(new { message = "Due date must be in the future" });
            var created = await _taskService.CreateAsync(dto);
            await _taskHub.Clients.All.SendAsync("TaskCreated", created.Id);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            await _taskHub.Clients.All.SendAsync("TaskUpdated", id);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _taskService.DeleteAsync(id);
            if (!ok) return NotFound();
            await _taskHub.Clients.All.SendAsync("TaskDeleted", id);
            return NoContent();
        }
    }
}