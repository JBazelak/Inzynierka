using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inzynierka.Application.Interfaces
{
    public interface IMaterialValidator
    {
        Task ValidateProjectExistsAsync(int contractorId, int projectId);
        Task ValidateMaterialCreationAsync(int contractorId, int projectId);
        void ValidateFile(IFormFile file);
    }
}
