using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;

public interface IAuthService
{
    Task RegisterAsync(RegisterContractorDto registerDto);
    Task<Contractor> LoginAsync(LoginContractorDto loginDto);
}
