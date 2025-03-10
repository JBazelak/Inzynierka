using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.Application.DTOs;
using Inzynierka.Application.Interfaces;

namespace Inzynierka.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ICompanyDataValidator _companyDataValidator;
        private readonly IMapper _mapper;

        public AuthService(IAuthRepository authRepository, ICompanyDataValidator contractorValidator, IMapper mapper)
        {
            _authRepository = authRepository;
            _companyDataValidator = contractorValidator;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterContractorDto registerDto)
        {
            await _companyDataValidator.ValidateCompanyDataAsync(registerDto.Email, registerDto.TaxIdNumber, registerDto.NationalBusinessRegistryNumber);

            var contractor = _mapper.Map<Contractor>(registerDto);
            contractor.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            await _authRepository.AddUserAsync(contractor);
        }

        public async Task<Contractor> LoginAsync(LoginContractorDto loginDto)
        {
            var contractor = await _authRepository.GetUserByEmailAsync(loginDto.Email);

            if (contractor == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, contractor.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return contractor;
        }
    }
}
