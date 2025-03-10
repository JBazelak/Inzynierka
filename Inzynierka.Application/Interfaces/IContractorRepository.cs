using Inzynierka.Application.DTOs;
using Inzynierka.Core.Entities;
namespace Inzynierka.Application.Interfaces
{
    public interface IContractorRepository
    {
        Task<Contractor> GetByIdAsync(int id);
        Task UpdateAsync(Contractor contractor);
        Task DeleteAsync(int id);
        Task<bool> ContractorExistsAsync(int contractorId);
        Task<bool> IsEmailTakenAsync(string email, int? excludeId = null);
        Task<bool> IsPhoneNumberTakenAsync(string phoneNumber, int? excludeId = null);
        Task<bool> IsCompanyNameTakenAsync(string companyName, int? excludeId = null);
        Task<bool> IsTaxIdNumberTakenAsync(string taxIdNumber, int? excludeId = null);
        Task<bool> IsNationalBusinessRegistryNumberTakenAsync(string registryNumber, int? excludeId = null);
    }
}
