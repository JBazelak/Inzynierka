using Inzynierka.UI.DTOs;

namespace Inzynierka.UI.Interfaces
{
    public interface IContractorService
    {
        Task<IEnumerable<ContractorDto>> GetAllAsync();
        Task<ContractorDto> GetByIdAsync(int id);
        Task<ContractorDto> CreateAsync(ContractorDto contractorDto);
        Task UpdateAsync(int id, UpdateContractorDto updateContractorDto);
        Task DeleteAsync(int id);
    }

}
