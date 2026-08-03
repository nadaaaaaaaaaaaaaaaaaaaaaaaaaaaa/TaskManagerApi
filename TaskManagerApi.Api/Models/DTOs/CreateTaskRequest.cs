using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Api.Models.DTOs
{
    public class CreateTaskRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}