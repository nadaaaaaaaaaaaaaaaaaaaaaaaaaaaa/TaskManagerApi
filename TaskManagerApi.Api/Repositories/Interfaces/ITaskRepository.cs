using TaskManagerApi.Api.Models.Entities;
namespace TaskManagerApi.Api.Repositories.Interfaces;

public interface ITaskRepository
{
    List<TaskItem> GetAll();
    TaskItem? GetById(int id);
    TaskItem Add(TaskItem task);
}
