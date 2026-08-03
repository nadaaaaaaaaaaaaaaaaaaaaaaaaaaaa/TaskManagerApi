using TaskManagerApi.Api.Models.Entities;

namespace TaskManagerApi.Api.Repositories.Interfaces;

public interface ITaskRepository
{
    IQueryable<TaskItem> Query();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(int id, string? title, string? description, bool? isCompleted);
    Task<bool> DeleteAsync(int id);
}