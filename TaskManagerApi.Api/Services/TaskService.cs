using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.Entities;
using TaskManagerApi.Api.Repositories.Interfaces;
using TaskManagerApi.Api.Services.Interfaces;

namespace TaskManagerApi.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        private static readonly Dictionary<string, Func<TaskItem, object>> _sortWhitelist = new(StringComparer.OrdinalIgnoreCase)
        {
            ["title"] = t => t.Title,
            ["createdAt"] = t => t.CreatedAt,
            ["isCompleted"] = t => t.IsCompleted,
        };

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public PagedResult<TaskItem> GetAllTasks(TaskFilterParams filterParams)
        {
            var query = _taskRepository.GetAll().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(filterParams.Search))
            {
                query = query.Where(t =>
                    t.Title.Contains(filterParams.Search, StringComparison.OrdinalIgnoreCase));
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

            query = query.OrderBy(_sortWhitelist[sortKey]);

            var totalCount = query.Count();

            var items = query
                .Skip((filterParams.Page - 1) * filterParams.PageSize)
                .Take(filterParams.PageSize)
                .ToList();

            return new PagedResult<TaskItem>
            {
                Items = items,
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                TotalCount = totalCount,
            };
        }

        public TaskItem? GetTaskById(int id) => _taskRepository.GetById(id);

        public TaskItem CreateTask(string title)
        {
            var task = new TaskItem
            {
                Title = title,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
            };
            return _taskRepository.Add(task);
        }
    }
}