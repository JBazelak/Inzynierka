using Inzynierka.Application.DTOs;
using Inzynierka.UI.Filters;
using Inzynierka.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.UI.Controllers
{
    [Route("api/contractors/{contractorId}/projects")]
    [SessionAuthorization]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(int contractorId)
        {
            try
            {
                var projects = await _projectService.GetAllAsync(contractorId);
                return Ok(projects);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int contractorId, int id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(contractorId, id);
                return Ok(project);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(int contractorId, [FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var project = await _projectService.CreateAsync(contractorId, projectDto);

                return CreatedAtAction(nameof(GetById), new { contractorId, id = project.Id }, project);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int contractorId, int id, [FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _projectService.UpdateAsync(contractorId, id, projectDto);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int contractorId, int id)
        {
            try
            {
                await _projectService.DeleteAsync(contractorId, id);
                return NoContent();
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

    }
}
