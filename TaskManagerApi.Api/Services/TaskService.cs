using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Api.Models;
using TaskManagerApi.Api.Models.DTOs;
using TaskManagerApi.Api.Models.Entities;
using TaskManagerApi.Api.Repositories.Interfaces;
using TaskManagerApi.Api.Services.Interfaces;

namespace TaskManagerApi.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        private static readonly Dictionary<string, Func<IQueryable<TaskItem>, IOrderedQueryable<TaskItem>>> _sortWhitelist =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["title"] = q => q.OrderBy(t => t.Title),
                ["createdAt"] = q => q.OrderBy(t => t.CreatedAt),
                ["isCompleted"] = q => q.OrderBy(t => t.IsCompleted),
            };

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<TaskSummaryDto>> GetAllTasksAsync(TaskFilterParams filterParams)
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

            return new PagedResult<TaskSummaryDto>
            {
                Items = _mapper.Map<List<TaskSummaryDto>>(items),
                Page = filterParams.Page,
                PageSize = filterParams.PageSize,
                TotalCount = totalCount,
            };
        }

        public async Task<TaskItemDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task is null ? null : _mapper.Map<TaskItemDto>(task);
        }

        public async Task<TaskItemDto> CreateTaskAsync(CreateTaskRequest request)
        {
            var task = _mapper.Map<TaskItem>(request);
            var created = await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskItemDto>(created);
        }

        public async Task<TaskItemDto?> UpdateTaskAsync(int id, UpdateTaskRequest request)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task is null)
                return null;

            _mapper.Map(request, task);
            task.UpdatedAt = DateTime.UtcNow;

            var updated = await _taskRepository.UpdateAsync(task);
            return updated is null ? null : _mapper.Map<TaskItemDto>(updated);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }
    }
}