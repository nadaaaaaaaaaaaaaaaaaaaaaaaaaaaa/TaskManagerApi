using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Api.Data;
using TaskManagerApi.Api.Models.Entities;
using TaskManagerApi.Api.Repositories.Interfaces;

namespace TaskManagerApi.Api.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<TaskItem> Query() => _context.Tasks.AsQueryable();

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        var existing = await _context.Tasks.FindAsync(task.Id);
        if (existing is null)
            return null;

        _context.Entry(existing).CurrentValues.SetValues(task);
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task is null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
}