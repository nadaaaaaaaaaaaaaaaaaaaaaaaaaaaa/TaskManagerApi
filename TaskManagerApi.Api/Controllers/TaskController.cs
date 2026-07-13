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

        // GET /api/tasks?search=meeting&isCompleted=false&page=1&pageSize=5&sortBy=title
        [HttpGet]
        public IActionResult GetAll([FromQuery] TaskFilterParams filterParams)
        {
            return Ok(_taskService.GetAllTasks(filterParams));
        }

        // GET /api/tasks/2
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task is null)
                return NotFound();
            return Ok(task);
        }

        // POST /api/tasks
        [HttpPost]
        public IActionResult Create([FromBody] TaskItem task)
        {
            var created = _taskService.CreateTask(task.Title);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }
}