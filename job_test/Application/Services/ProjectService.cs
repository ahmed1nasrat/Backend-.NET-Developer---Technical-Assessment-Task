using job_test.Application.DTOs.Projects;
using job_test.Application.Interfaces;
using job_test.Domain.Models;
using job_test.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace job_test.Application.Services
{
    public class ProjectService: IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project> CreateAsync( CreateProjectDto dto,int userId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            _context.Projects.Add(project);

            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<List<Project>> GetAllAsync(int userId)
        {
            return await _context.Projects .Where(x => x.UserId == userId) .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(
            int id,
            int userId)
        {
            return await _context.Projects.FirstOrDefaultAsync(x => x.Id == id &&  x.UserId == userId);
        }

        public async Task<Project?> UpdateAsync( int id, UpdateProjectDto dto,int userId)
        {
            var project = await _context.Projects .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                return null;
            }

            project.Name = dto.Name;
            project.Description = dto.Description;

            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<bool> DeleteAsync( int id, int userId)
        {
            var project = await _context.Projects .FirstOrDefaultAsync(x =>  x.Id == id &&  x.UserId == userId);

            if (project == null)
            {
                return false;
            }

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
