using Inzynierka.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task AddUserAsync(Contractor contractor);
        Task<Contractor?> GetUserByEmailAsync(string email);
    }

}
