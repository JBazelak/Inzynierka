using AutoMapper;
using Inzynierka.Core.Entities;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.UI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Inzynierka.UI.Services
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
