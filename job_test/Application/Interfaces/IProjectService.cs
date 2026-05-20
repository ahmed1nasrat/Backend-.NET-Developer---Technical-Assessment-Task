using job_test.Application.DTOs.Projects;
using job_test.Domain.Models;

namespace job_test.Application.Interfaces
{
    public interface IProjectService
    {
        Task<Project> CreateAsync( CreateProjectDto dto, int userId);

        Task<List<Project>> GetAllAsync(int userId);

        Task<Project?> GetByIdAsync(   int id,  int userId);

        Task<Project?> UpdateAsync(  int id,   UpdateProjectDto dto,  int userId);

        Task<bool> DeleteAsync(  int id,  int userId);
    }
}

