using AutoMapper;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Core.Entities;
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

        public async Task<IEnumerable<ContractorDto>> GetAllAsync()
        {
            var contractors = await _context.Contractors.ToListAsync();
            return _mapper.Map<IEnumerable<ContractorDto>>(contractors);
        }

        public async Task<ContractorDto> GetByIdAsync(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            if (contractor == null)
            {
                throw new KeyNotFoundException("Contractor not found.");
            }
            return _mapper.Map<ContractorDto>(contractor);
        }

        public async Task<ContractorDto> CreateAsync(ContractorDto contractorDto)
        {
            if (await IsEmailTakenAsync(contractorDto.Email))
            {
                throw new ArgumentException("Email is already taken.");
            }

            if (await IsPhoneNumberTakenAsync(contractorDto.PhoneNumber))
            {
                throw new ArgumentException("Phone number is already taken.");
            }

            if (await IsCompanyNameTakenAsync(contractorDto.CompanyName))
            {
                throw new ArgumentException("Company name is already taken.");
            }

            var contractor = _mapper.Map<Contractor>(contractorDto);
            _context.Contractors.Add(contractor);
            await _context.SaveChangesAsync();

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
    }
}
