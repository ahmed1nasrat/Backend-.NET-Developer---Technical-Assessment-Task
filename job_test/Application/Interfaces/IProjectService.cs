using job_test.Application.DTOs.Projects;
using job_test.Domain.Models;

namespace job_test.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResponseDto> CreateAsync( CreateProjectDto dto, int userId);

        Task<List<ProjectResponseDto>> GetAllAsync(int userId);

        Task<ProjectResponseDto?> GetByIdAsync( int id, int userId);

        Task<ProjectResponseDto?> UpdateAsync( int id,  UpdateProjectDto dto, int userId);

        Task<bool> DeleteAsync( int id, int userId);
    }
}

