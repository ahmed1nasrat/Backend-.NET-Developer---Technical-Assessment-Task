using job_test.Application.DTOs.Tasks;
using job_test.Domain.Models;

namespace job_test.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskItem> CreateAsync(  CreateTaskDto dto, int userId);

        Task<List<TaskItem>> GetByProjectAsync( int projectId, int userId);

        Task<TaskItem?> GetByIdAsync( int taskId,  int userId);

        Task<TaskItem?> UpdateAsync( int taskId,UpdateTaskDto dto, int userId);

        Task<TaskItem?> UpdateStatusAsync( int taskId,  UpdateTaskStatusDto dto, int userId);

        Task<bool> DeleteAsync( int taskId,int userId);
    }
}
