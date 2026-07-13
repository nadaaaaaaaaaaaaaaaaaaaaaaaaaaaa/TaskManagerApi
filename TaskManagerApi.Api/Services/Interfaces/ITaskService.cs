using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.Entities;

namespace TaskManagerApi.Api.Services.Interfaces
{
    public interface ITaskService
    {
        PagedResult<TaskItem> GetAllTasks(TaskFilterParams filterParams);
        TaskItem? GetTaskById(int id);
        TaskItem CreateTask(string title);
    }
}