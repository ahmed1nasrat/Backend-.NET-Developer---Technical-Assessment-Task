using job_test.Domain.Enums;

namespace job_test.Domain.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ProjectTaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }

        public TaskPriority Priority { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

    }
}
    