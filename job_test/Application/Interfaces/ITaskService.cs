using job_test.Application.DTOs.Tasks;

namespace job_test.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto, int userId);

        Task<List<TaskResponseDto>> GetByProjectAsync(int projectId, int userId);

        Task<TaskResponseDto?> GetByIdAsync(int taskId, int userId);

        Task<TaskResponseDto?> UpdateAsync(int taskId, UpdateTaskDto dto, int userId);

        Task<TaskResponseDto?> UpdateStatusAsync(int taskId, UpdateTaskStatusDto dto, int userId);

        Task<bool> DeleteAsync(int taskId, int userId);
    }
}
