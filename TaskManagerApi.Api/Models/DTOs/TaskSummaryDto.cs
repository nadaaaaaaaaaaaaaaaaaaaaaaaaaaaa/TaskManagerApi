namespace TaskManagerApi.Api.Models.DTOs
{
    public class TaskSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}