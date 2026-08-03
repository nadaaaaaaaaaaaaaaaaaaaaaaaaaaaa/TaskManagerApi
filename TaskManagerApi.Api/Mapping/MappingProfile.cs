using AutoMapper;
using TaskManagerApi.Api.Models.DTOs;
using TaskManagerApi.Api.Models.Entities;

namespace TaskManagerApi.Api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity -> DTO (read)
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItem, TaskSummaryDto>();

            // Request -> Entity (create)
            CreateMap<CreateTaskRequest, TaskItem>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // Request -> Entity (update) — only overwrite fields the client actually sent
            CreateMap<UpdateTaskRequest, TaskItem>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.IsCompleted, opt => opt.Condition(src => src.IsCompleted.HasValue))
                .ForMember(dest => dest.DueDate, opt => opt.Condition(src => src.DueDate.HasValue));
        }
    }
}