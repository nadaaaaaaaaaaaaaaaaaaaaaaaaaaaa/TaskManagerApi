using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.DTOs;
using TaskManagerApi.Api.Services.Interfaces;

namespace TaskManagerApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Retrieves a paginated, filtered, and sorted list of tasks.
        /// </summary>
        /// <param name="filterParams">
        /// Query parameters for filtering (search, isCompleted, createdAfter, createdBefore),
        /// sorting (sortBy), and pagination (page, pageSize).
        /// </param>
        /// <response code="200">Returns the paginated list of task summaries matching the filter criteria.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TaskSummaryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] TaskFilterParams filterParams)
        {
            var result = await _taskService.GetAllTasksAsync(filterParams);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a single task by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <response code="200">Returns the requested task.</response>
        /// <response code="404">No task exists with the specified id.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task is null)
                return NotFound();
            return Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="request">The task details to create.</param>
        /// <response code="201">The task was created successfully.</response>
        /// <response code="400">The request body was invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
        {
            var created = await _taskService.CreateTaskAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Updates an existing task.
        /// </summary>
        /// <param name="id">The unique identifier of the task to update.</param>
        /// <param name="request">The fields to update.</param>
        /// <response code="200">The task was updated successfully.</response>
        /// <response code="404">No task exists with the specified id.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskRequest request)
        {
            var updated = await _taskService.UpdateTaskAsync(id, request);
            if (updated is null)
                return NotFound();
            return Ok(updated);
        }

        /// <summary>
        /// Deletes a task.
        /// </summary>
        /// <param name="id">The unique identifier of the task to delete.</param>
        /// <response code="204">The task was deleted successfully.</response>
        /// <response code="404">No task exists with the specified id.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteTaskAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}