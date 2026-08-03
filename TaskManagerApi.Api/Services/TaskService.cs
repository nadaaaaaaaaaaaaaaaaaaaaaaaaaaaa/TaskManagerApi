using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.Entities;
using TaskManagerApi.Api.Repositories.Interfaces;
using TaskManagerApi.Api.Services.Interfaces;

namespace TaskManagerApi.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        private static readonly Dictionary<string, Func<IQueryable<TaskItem>, IOrderedQueryable<TaskItem>>> _sortWhitelist =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["title"] = q => q.OrderBy(t => t.Title),
                ["createdAt"] = q => q.OrderBy(t => t.CreatedAt),
                ["isCompleted"] = q => q.OrderBy(t => t.IsCompleted),
            };

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<PagedResult<TaskItem>> GetAllTasksAsync(TaskFilterParams filterParams)
        {
            var query = _taskRepository.Query();

            if (!string.IsNullOrWhiteSpace(filterParams.Search))
            {
                var search = filterParams.Search.ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(search));
            }

            if (filterParams.IsCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == filterParams.IsCompleted.Value);
            }

            if (filterParams.CreatedAfter.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= filterParams.CreatedAfter.Value);
            }

            if (filterParams.CreatedBefore.HasValue)
            {
                query = query.Where(t => t.CreatedAt <= filterParams.CreatedBefore.Value);
            }

            var sortKey = !string.IsNullOrWhiteSpace(filterParams.SortBy) && _sortWhitelist.ContainsKey(filterParams.SortBy)
                ? filterParams.SortBy
                : "createdAt";

            query = _sortWhitelist[sortKey](query);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filterParams.Page - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToListAsync();

            return new PagedResult<TaskItem>
            {
                Items = items,
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                TotalCount = totalCount,
            };
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id) => await _taskRepository.GetByIdAsync(id);

        public async Task<TaskItem> CreateTaskAsync(string title, int userId, string? description)
        {
            var task = new TaskItem
            {
                Title = title,
                Description = description,
                UserId = userId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
            };
            return await _taskRepository.AddAsync(task);
        }

        public async Task<TaskItem?> UpdateTaskAsync(int id, string? title, string? description, bool? isCompleted)
        {
            return await _taskRepository.UpdateAsync(id, title, description, isCompleted);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
    }
}