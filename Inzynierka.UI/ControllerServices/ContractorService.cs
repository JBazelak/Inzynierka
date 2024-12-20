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
    }
}