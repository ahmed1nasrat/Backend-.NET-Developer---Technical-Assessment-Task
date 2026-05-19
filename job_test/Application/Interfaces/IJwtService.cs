namespace job_test.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken( int userId,string userName,string email);
    
    }
}
