using job_test.Application.DTOs.Tasks;
using job_test.Application.Exceptions;
using job_test.Application.Interfaces;
using job_test.Domain.Enums;
using job_test.Domain.Models;
using job_test.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace job_test.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static TaskResponseDto MapToDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status,
                ProjectId = task.ProjectId
            };
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto,int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == dto.ProjectId &&x.UserId == userId);

            if (project == null)
            {
                throw new NotFoundException("Project not found");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                Status = ProjectTaskStatus.ToDo,
                ProjectId = dto.ProjectId
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<List<TaskResponseDto>> GetByProjectAsync(int projectId, int userId, ProjectTaskStatus? status = null)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.UserId == userId);

            if (project == null)
            {
                throw new NotFoundException("Project not found");
            }

            var query = _context.Tasks.Where(x => x.ProjectId == projectId);

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            var tasks = await query.ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<List<TaskResponseDto>> GetAllAsync(int userId)
        {
            var tasks = await _context.Tasks
                .Include(x => x.Project)
                .Where(x => x.Project.UserId == userId)
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<List<TaskResponseDto>> GetByStatusAsync(ProjectTaskStatus status, int userId)
        {
            var tasks = await _context.Tasks
                .Include(x => x.Project)
                .Where(x => x.Project.UserId == userId && x.Status == status)
                .ToListAsync();

            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskResponseDto> GetByIdAsync(int taskId, int userId)
        {
            var task = await _context.Tasks .Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == taskId && x.Project.UserId == userId);

            if (task == null)
            {
                throw new NotFoundException( "Task not found");
            }

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateAsync( int taskId, UpdateTaskDto dto, int userId)
        {
            var task = await _context.Tasks.Include(x => x.Project) .FirstOrDefaultAsync(x => x.Id == taskId &&x.Project.UserId == userId);

            if (task == null)
            {
                throw new NotFoundException("Task not found");
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.Priority = dto.Priority;
            task.Status = dto.Status;

            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateStatusAsync(int taskId, UpdateTaskStatusDto dto, int userId)
        {
            var task = await _context.Tasks .Include(x => x.Project) .FirstOrDefaultAsync(x =>x.Id == taskId && x.Project.UserId == userId);

            if (task == null)
            {
                throw new NotFoundException( "Task not found");
            }

            task.Status = dto.Status;

            await _context.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<bool> DeleteAsync( int taskId,int userId)
        {
            var task = await _context.Tasks .Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == taskId &&x.Project.UserId == userId);

            if (task == null)
            {
                throw new NotFoundException( "Task not found");
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}