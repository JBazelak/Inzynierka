using Inzynierka.UI.DTOs;
using Inzynierka.UI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.UI.Controllers
{
    [Route("api/contractors/{contractorId}/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreateProjectDto>>> GetAll(int contractorId)
        {
            var projects = await _projectService.GetAllAsync(contractorId);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CreateProjectDto>> GetById(int contractorId, int id)
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
                // Wywołanie serwisu
                var project = await _projectService.CreateAsync(contractorId, projectDto);

                // Zwracamy odpowiedź z URL do nowego zasobu
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
        [HttpPost("{projectId}/materials")]
        public async Task<ActionResult> AddMaterial(int contractorId, int projectId, [FromBody] CreateMaterialDto createMaterialDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var material = await _projectService.AddMaterialAsync(contractorId, projectId, createMaterialDto);
                return Created($"api/contractors/{contractorId}/projects/{projectId}/materials/{material.Id}", material);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{projectId}/materials/{materialId}")]
        public async Task<ActionResult> UpdateMaterial(int contractorId, int projectId, int materialId, [FromBody] UpdateMaterialDto updateMaterialDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _projectService.UpdateMaterialAsync(contractorId, projectId, materialId, updateMaterialDto);
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


    }
}
