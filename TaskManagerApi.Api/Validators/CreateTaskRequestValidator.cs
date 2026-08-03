using FluentValidation;
using TaskManagerApi.Api.Models.DTOs;

namespace TaskManagerApi.Api.Validators
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
                .Must(NotContainHtmlTags).WithMessage("Title must not contain HTML tags.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.")
                .When(x => x.DueDate.HasValue);
        }

        private static bool NotContainHtmlTags(string title)
        {
            return !System.Text.RegularExpressions.Regex.IsMatch(title, "<.*?>");
        }
    }
}