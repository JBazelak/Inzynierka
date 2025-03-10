using Inzynierka.Application.DTOs;

namespace Inzynierka.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync(int contractorId);
        Task<ProjectDto> GetByIdAsync(int contractorId, int id);
        Task<ProjectDto> CreateAsync(int contractorId, CreateProjectDto projectDto);
        Task UpdateAsync(int contractorId, int id, CreateProjectDto projectDto);
        Task DeleteAsync(int contractorId, int id);
    }
}
