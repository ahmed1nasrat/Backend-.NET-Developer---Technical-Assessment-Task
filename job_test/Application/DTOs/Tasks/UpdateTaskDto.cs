using job_test.Domain.Enums;

namespace job_test.Application.DTOs.Tasks
{
    public class UpdateTaskDto
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public ProjectTaskStatus Status { get; set; }
    }
}
