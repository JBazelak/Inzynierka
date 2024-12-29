using Inzynierka.UI.DTOs;

namespace Inzynierka.UI.Interfaces
{
    public interface IContractorService
    {
        Task<ContractorDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, UpdateContractorDto updateContractorDto);
        Task DeleteAsync(int id);
    }

}
