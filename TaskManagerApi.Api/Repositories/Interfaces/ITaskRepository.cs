using TaskManagerApi.Api.Models.Entities;

namespace TaskManagerApi.Api.Repositories.Interfaces;

public interface ITaskRepository
{
    IQueryable<TaskItem> Query();
    Task<TaskItem?> GetByIdAsync(int id);
    Task<TaskItem> AddAsync(TaskItem task);
    Task<TaskItem?> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(int id);
}