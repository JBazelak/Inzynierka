using AutoMapper;
using Inzynierka.Application.DTOs;
using Inzynierka.Application.Interfaces;

public class ContractorService : IContractorService
{
    private readonly IContractorRepository _contractorRepository;
    private readonly IMapper _mapper;

    public ContractorService(IContractorRepository contractorRepository, IMapper mapper)
    {
        _contractorRepository = contractorRepository;
        _mapper = mapper;
    }

    public async Task<ContractorDto> GetByIdAsync(int id)
    {
        var contractor = await _contractorRepository.GetByIdAsync(id);
        if (contractor == null)
            throw new KeyNotFoundException("Contractor not found.");

        return _mapper.Map<ContractorDto>(contractor);
    }

    public async Task UpdateAsync(int id, UpdateContractorDto updateContractorDto)
    {
        var contractor = await _contractorRepository.GetByIdAsync(id);
        if (contractor == null)
            throw new KeyNotFoundException("Contractor not found.");

        if (!string.IsNullOrEmpty(updateContractorDto.Email) &&
            await _contractorRepository.IsEmailTakenAsync(updateContractorDto.Email, id))
        {
            throw new ArgumentException("Email is already taken.");
        }

        _mapper.Map(updateContractorDto, contractor);
        await _contractorRepository.UpdateAsync(contractor);
    }

    public async Task DeleteAsync(int id)
    {
        await _contractorRepository.DeleteAsync(id);
    }

    public async Task<bool> ContractorExistsAsync(int contractorId)
    {
        return await _contractorRepository.ContractorExistsAsync(contractorId);
    }
}
