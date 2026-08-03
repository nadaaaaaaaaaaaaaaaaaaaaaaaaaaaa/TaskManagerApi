using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.Entities;
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
        /// <response code="200">Returns the paginated list of tasks matching the filter criteria.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<TaskItem>), StatusCodes.Status200OK)]
        public IActionResult GetAll([FromQuery] TaskFilterParams filterParams)
        {
            return Ok(_taskService.GetAllTasks(filterParams));
        }

        /// <summary>
        /// Retrieves a single task by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <response code="200">Returns the requested task.</response>
        /// <response code="404">No task exists with the specified id.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task is null)
                return NotFound();
            return Ok(task);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <param name="task">The task details to create. Only the Title is currently used.</param>
        /// <response code="201">The task was created successfully. Returns the created task and its location.</response>
        /// <response code="400">The request body was invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(TaskItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] TaskItem task)
        {
            var created = _taskService.CreateTask(task.Title);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}