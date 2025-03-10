using Inzynierka.Application.DTOs;

namespace Inzynierka.Application.Interfaces
{
    public interface IContractorService
    {
        Task<ContractorDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateContractorDto updateContractorDto);
        Task DeleteAsync(int id);
        Task<bool> ContractorExistsAsync(int contractorId);
    }

}
