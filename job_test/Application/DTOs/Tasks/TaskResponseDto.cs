using job_test.Domain.Enums;

namespace job_test.Application.DTOs.Tasks
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public ProjectTaskStatus Status { get; set; }

        public int ProjectId { get; set; }
    }
}
