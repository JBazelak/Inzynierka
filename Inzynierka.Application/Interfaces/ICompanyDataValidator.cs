using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Application.Interfaces
{
    public interface ICompanyDataValidator
    {
        public Task ValidateCompanyDataAsync(string? email, string? taxIdNumber, string? registryNumber, int? excluded = null);
    }
}
