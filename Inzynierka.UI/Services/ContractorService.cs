using AutoMapper;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.UI.Interfaces;
using Inzynierka.UI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.UI.ControllerServices
{
    public class ContractorService : IContractorService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContractorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ContractorDto> GetByIdAsync(int id)
        {
            var contractor = await _context.Contractors
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contractor == null)
                throw new KeyNotFoundException("Contractor not found.");

            return _mapper.Map<ContractorDto>(contractor);
        }


        public async Task UpdateAsync(int id, UpdateContractorDto updateContractorDto)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                throw new KeyNotFoundException("Contractor not found.");
            }

            if (!string.IsNullOrEmpty(updateContractorDto.Email) &&
                await IsEmailTakenAsync(updateContractorDto.Email, id))
            {
                throw new ArgumentException("Email is already taken.");
            }

            if (!string.IsNullOrEmpty(updateContractorDto.PhoneNumber) &&
                await IsPhoneNumberTakenAsync(updateContractorDto.PhoneNumber, id))
            {
                throw new ArgumentException("Phone number is already taken.");
            }

            if (!string.IsNullOrEmpty(updateContractorDto.CompanyName) &&
                await IsCompanyNameTakenAsync(updateContractorDto.CompanyName, id))
            {
                throw new ArgumentException("Company name is already taken.");
            }

            if (!string.IsNullOrEmpty(updateContractorDto.TaxIdNumber) &&
                await IsTaxIdNumberTakenAsync(updateContractorDto.TaxIdNumber, id))
            {
                throw new ArgumentException("Tax ID Number (NIP) is already taken.");
            }

            if (!string.IsNullOrEmpty(updateContractorDto.NationalBusinessRegistryNumber) &&
                await IsNationalBusinessRegistryNumberTakenAsync(updateContractorDto.NationalBusinessRegistryNumber, id))
            {
                throw new ArgumentException("National Business Registry Number (REGON) is already taken.");
            }


            _mapper.Map(updateContractorDto, contractor);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                throw new KeyNotFoundException("Contractor not found.");
            }

            _context.Contractors.Remove(contractor);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ContractorExistsAsync(int contractorId)
        {
            return await _context.Contractors.AnyAsync(c => c.Id == contractorId);
        }
        private async Task<bool> IsEmailTakenAsync(string email, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.Email == email && (excludeId == null || c.Id != excludeId));
        }

        private async Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.PhoneNumber == phoneNumber && (excludeId == null || c.Id != excludeId));
        }

        private async Task<bool> IsCompanyNameTakenAsync(string companyName, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.CompanyName == companyName && (excludeId == null || c.Id != excludeId));
        }

        private async Task<bool> IsTaxIdNumberTakenAsync(string taxIdNumber, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.TaxIdNumber == taxIdNumber && (excludeId == null || c.Id != excludeId));
        }

        private async Task<bool> IsNationalBusinessRegistryNumberTakenAsync(string registryNumber, int? excludeId = null)
        {
            return await _context.Contractors
                .AnyAsync(c => c.NationalBusinessRegistryNumber == registryNumber && (excludeId == null || c.Id != excludeId));
        }

    }
}
