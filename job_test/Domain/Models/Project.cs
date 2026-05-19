namespace job_test.Domain.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } 

        public string Description { get; set; } 

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } 

        public ICollection<TaskItem> Tasks { get; set; }
    }
}
