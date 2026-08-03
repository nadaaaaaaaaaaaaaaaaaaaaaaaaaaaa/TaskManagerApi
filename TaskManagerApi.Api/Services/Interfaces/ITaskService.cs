using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.Entities;

namespace TaskManagerApi.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<PagedResult<TaskItem>> GetAllTasksAsync(TaskFilterParams filterParams);
        Task<TaskItem?> GetTaskByIdAsync(int id);
        Task<TaskItem> CreateTaskAsync(string title, int userId, string? description);
        Task<TaskItem?> UpdateTaskAsync(int id, string? title, string? description, bool? isCompleted);
        Task<bool> DeleteTaskAsync(int id);
    }
}