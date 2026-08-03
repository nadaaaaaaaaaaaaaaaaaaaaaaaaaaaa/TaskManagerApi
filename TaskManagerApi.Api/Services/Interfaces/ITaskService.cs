using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.DTOs;

namespace TaskManagerApi.Api.Services.Interfaces
{
    public interface ITaskService
    {
        Task<PagedResult<TaskSummaryDto>> GetAllTasksAsync(TaskFilterParams filterParams);
        Task<TaskItemDto?> GetTaskByIdAsync(int id);
        Task<TaskItemDto> CreateTaskAsync(CreateTaskRequest request);
        Task<TaskItemDto?> UpdateTaskAsync(int id, UpdateTaskRequest request);
        Task<bool> DeleteTaskAsync(int id);
    }
}