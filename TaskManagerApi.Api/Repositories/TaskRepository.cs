using TaskManagerApi.Api.Models.Entities;
using TaskManagerApi.Api.Repositories.Interfaces;

namespace TaskManagerApi.Api.Repositories;

public class TaskRepository : ITaskRepository
{
    private static readonly List<TaskItem> _tasks = new()
    {
        new TaskItem { Id = 1, Title = "Buy groceries", IsCompleted = false, CreatedAt = DateTime.UtcNow },
        new TaskItem { Id = 2, Title = "Read a book", IsCompleted = true, CreatedAt = DateTime.UtcNow },
    };

    public List<TaskItem> GetAll() => _tasks;

    public TaskItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

    public TaskItem Add(TaskItem task)
    {
        task.Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
        _tasks.Add(task);
        return task;
    }
}