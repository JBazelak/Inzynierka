using Inzynierka.Core.Entities;
using Inzynierka.Application.Interfaces;
using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync(int contractorId)
        {
            return await _context.Projects
                .Include(p => p.Materials)
                .Where(p => p.ContractorId == contractorId)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(int contractorId, int id)
        {
            return await _context.Projects
                .Include(p => p.Materials)
                .FirstOrDefaultAsync(p => p.ContractorId == contractorId && p.Id == id);
        }

        public async Task AddAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetProjectCostAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.Materials)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            return project.Materials.Sum(m => m.TotalCost);
        }
    }
}
