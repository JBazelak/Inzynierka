using Inzynierka.Application.Interfaces;
using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Inzynierka.Core.Entities;

namespace Inzynierka.Infrastructure.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly AppDbContext _context;

        public ContractorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Contractor> GetByIdAsync(int id)
        {
            return await _context.Contractors
                .Include(c => c.Projects) 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Contractor contractor)
        {
            _context.Contractors.Update(contractor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contractor = await GetByIdAsync(id);
            if (contractor != null)
            {
                _context.Contractors.Remove(contractor);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ContractorExistsAsync(int contractorId)
        {
            return await _context.Contractors.AnyAsync(c => c.Id == contractorId);
        }

        public async Task<bool> IsEmailTakenAsync(string email, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.Email == email && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.PhoneNumber == phoneNumber && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> IsCompanyNameTakenAsync(string companyName, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.CompanyName == companyName && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> IsTaxIdNumberTakenAsync(string taxIdNumber, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.TaxIdNumber == taxIdNumber && (excludeId == null || c.Id != excludeId));
        }

        public async Task<bool> IsNationalBusinessRegistryNumberTakenAsync(string registryNumber, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.NationalBusinessRegistryNumber == registryNumber && (excludeId == null || c.Id != excludeId));
        }
    }
}
