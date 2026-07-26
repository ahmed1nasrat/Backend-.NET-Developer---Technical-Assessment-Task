using job_test.Application.DTOs.Tasks;
using job_test.Application.Interfaces;
using job_test.Domain.Enums;
using job_test.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace job_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            var userId = User.GetUserId();

            var result = await _taskService.CreateAsync(dto, userId);

            return Ok(result);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetByProject(int projectId, [FromQuery] ProjectTaskStatus? status = null)
        {
            var userId = User.GetUserId();

            var result = await _taskService.GetByProjectAsync(projectId, userId, status);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProjectTaskStatus? status = null)
        {
            var userId = User.GetUserId();

            if (status.HasValue)
            {
                var filtered = await _taskService.GetByStatusAsync(status.Value, userId);
                return Ok(filtered);
            }

            return Ok(Array.Empty<TaskResponseDto>());
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetById(int taskId)
        {
            var userId = User.GetUserId();

            var result = await _taskService.GetByIdAsync(taskId, userId);

            return Ok(result);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> Update(int taskId, UpdateTaskDto dto)
        {
            var userId = User.GetUserId();

            var result = await _taskService.UpdateAsync(taskId, dto, userId);

            return Ok(result);
        }

        [HttpPatch("{taskId}/status")]
        public async Task<IActionResult> UpdateStatus(int taskId, UpdateTaskStatusDto dto)
        {
            var userId = User.GetUserId();

            var result = await _taskService.UpdateStatusAsync(taskId, dto, userId);

            return Ok(result);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> Delete(int taskId)
        {
            var userId = User.GetUserId();

            await _taskService.DeleteAsync(taskId, userId);

            return Ok("Task deleted successfully");
        }
    }
}
