using job_test.Application.DTOs.Tasks;
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

        public async Task<TaskItem> CreateAsync( CreateTaskDto dto, int userId)
        {
            var project = await _context.Projects  .FirstOrDefaultAsync(x =>  x.Id == dto.ProjectId && x.UserId == userId);

            if (project == null)
            {
                throw new Exception("Project not found");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                Status = ProjectTaskStatus.Pending,
                ProjectId = dto.ProjectId
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<List<TaskItem>> GetByProjectAsync( int projectId, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId && x.UserId == userId);

            if (project == null)
            {
                throw new Exception("Project not found");
            }

            return await _context.Tasks.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync( int taskId, int userId)
        {
            return await _context.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x =>x.Id == taskId &&x.Project.UserId == userId);
        }

        public async Task<TaskItem?> UpdateAsync( int taskId, UpdateTaskDto dto, int userId)
        {
            var task = await _context.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == taskId && x.Project.UserId == userId);

            if (task == null)
            {
                return null;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.Priority = dto.Priority;
            task.Status = dto.Status;

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<TaskItem?> UpdateStatusAsync( int taskId, UpdateTaskStatusDto dto, int userId)
        {
            var task = await _context.Tasks.Include(x => x.Project).FirstOrDefaultAsync(x => x.Id == taskId && x.Project.UserId == userId);

            if (task == null)
            {
                return null;
            }

            task.Status = dto.Status;

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteAsync( int taskId, int userId)
        {
            var task = await _context.Tasks .Include(x => x.Project) .FirstOrDefaultAsync(x => x.Id == taskId && x.Project.UserId == userId);

            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
