using Inzynierka.Application.Interfaces;
using Inzynierka.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.Infrastructure.Validators
{
    class CompanyDataValidator : ICompanyDataValidator
    {
        private readonly AppDbContext _context;

        public CompanyDataValidator(AppDbContext context)
        {
            _context = context;
        }

        public async Task ValidateCompanyDataAsync(string? email, string? taxIdNumber, string? registryNumber, int? excludeId = null)
        {
            if (!string.IsNullOrEmpty(email) &&
                await _context.Contractors.AnyAsync(c => c.Email == email && (excludeId == null || c.Id != excludeId)))
            {
                throw new ArgumentException("Email already exists.");
            }

            if (!string.IsNullOrEmpty(taxIdNumber) &&
                await _context.Contractors.AnyAsync(c => c.TaxIdNumber == taxIdNumber && (excludeId == null || c.Id != excludeId)))
            {
                throw new ArgumentException("Tax ID Number (NIP) already exists.");
            }

            if (!string.IsNullOrEmpty(registryNumber) &&
                await _context.Contractors.AnyAsync(c => c.NationalBusinessRegistryNumber == registryNumber && (excludeId == null || c.Id != excludeId)))
            {
                throw new ArgumentException("National Business Registry Number (REGON) already exists.");
            }
        }
    }
}

