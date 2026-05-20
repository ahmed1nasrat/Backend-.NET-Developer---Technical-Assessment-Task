using job_test.Application.DTOs.Projects;
using job_test.Application.Exceptions;
using job_test.Application.Interfaces;
using job_test.Domain.Models;
using job_test.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace job_test.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static ProjectResponseDto MapToDto(Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto,int userId)
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

            return MapToDto(project);
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync( int userId)
        {
            var projects = await _context.Projects .Where(x => x.UserId == userId).ToListAsync();

            return projects.Select(MapToDto) .ToList();
        }

        public async Task<ProjectResponseDto> GetByIdAsync( int id, int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                throw new NotFoundException("Project not found");
            }

            return MapToDto(project);
        }

        public async Task<ProjectResponseDto> UpdateAsync( int id, UpdateProjectDto dto, int userId)
        {
            var project = await _context.Projects .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                throw new NotFoundException( "Project not found");
            }

            project.Name = dto.Name;
            project.Description = dto.Description;

            await _context.SaveChangesAsync();

            return MapToDto(project);
        }

        public async Task<bool> DeleteAsync( int id,int userId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

            if (project == null)
            {
                throw new NotFoundException( "Project not found");
            }

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}