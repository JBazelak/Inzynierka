using Inzynierka.Core.Entities;
using Inzynierka.Application.Interfaces;
using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(Contractor contractor)
        {
            _context.Contractors.Add(contractor);
            await _context.SaveChangesAsync();
        }

        public async Task<Contractor?> GetUserByEmailAsync(string email)
        {
            return await _context.Contractors.FirstOrDefaultAsync(c => c.Email == email);
        }
    }
}
