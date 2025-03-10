using Inzynierka.Core.Entities;
using Inzynierka.Application.Interfaces;
using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.Infrastructure.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync(int projectId)
        {
            return await _context.Materials
                .Where(m => m.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Material?> GetMaterialAsync(int projectId, int materialId)
        {
            return await _context.Materials
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.ProjectId == projectId && m.Id == materialId);
        }

        public async Task AddMaterialAsync(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMaterialAsync(Material material)
        {
            _context.Materials.Update(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMaterialAsync(Material material)
        {
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}
