namespace job_test.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; } 

        public string PasswordHash { get; set; } 

        public ICollection<Project> Projects { get; set; } 
    }
}
