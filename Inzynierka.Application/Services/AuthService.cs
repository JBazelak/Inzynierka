using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task RegisterAsync(RegisterContractorDto registerDto)
        {
            if (await _context.Contractors.AnyAsync(c => c.Email == registerDto.Email))
            {
                throw new ArgumentException("Email already exists.");
            }

            if (!string.IsNullOrEmpty(registerDto.TaxIdNumber) &&
                await _context.Contractors.AnyAsync(c => c.TaxIdNumber == registerDto.TaxIdNumber))
            {
                throw new ArgumentException("Tax ID Number (NIP) already exists.");
            }

            if (!string.IsNullOrEmpty(registerDto.NationalBusinessRegistryNumber) &&
                await _context.Contractors.AnyAsync(c => c.NationalBusinessRegistryNumber == registerDto.NationalBusinessRegistryNumber))
            {
                throw new ArgumentException("National Business Registry Number (REGON) already exists.");
            }

            var contractor = _mapper.Map<Contractor>(registerDto);

            contractor.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            _context.Contractors.Add(contractor);
            await _context.SaveChangesAsync();
        }


        public async Task<Contractor> LoginAsync(LoginContractorDto loginDto)
        {
            var contractor = await _context.Contractors
                .FirstOrDefaultAsync(c => c.Email == loginDto.Email);

            if (contractor == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, contractor.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return contractor;
        }
    }
}