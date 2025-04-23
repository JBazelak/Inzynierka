using Inzynierka.Application.DTOs;
using Inzynierka.Application.Interfaces;
using Inzynierka.Application.Services;
using Inzynierka.UI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.UI.Controllers
{
    [Route("api/contractors/{contractorId}/projects/{projectId}/materials/")]
    [SessionAuthorization]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IContractorRepository _contractorService;

        public MaterialController(IMaterialService materialService, IContractorRepository contractorService)
        {
            _materialService = materialService;
            _contractorService = contractorService;
        }

        private async Task<bool> ContractorExistsAsync(int contractorId)
        {
            return await _contractorService.ContractorExistsAsync(contractorId);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetAllMaterials(int contractorId, int projectId)
        {
            if (!await ContractorExistsAsync(contractorId))
            {
                return NotFound("Contractor not found.");
            }

            try
            {
                var materials = await _materialService.GetAllMaterialsAsync(projectId);
                return Ok(materials);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddMaterial(int contractorId, int projectId, [FromBody] CreateMaterialDto createMaterialDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var material = await _materialService.AddMaterialAsync(contractorId, projectId, createMaterialDto);
                return Created($"api/contractors/{contractorId}/projects/{projectId}/materials/{material.Id}", material);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{materialId}")]
        public async Task<ActionResult> UpdateMaterial(int contractorId, int projectId, int materialId, [FromBody] UpdateMaterialDto updateMaterialDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _materialService.UpdateMaterialAsync(contractorId, projectId, materialId, updateMaterialDto);
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


        [HttpPost("{materialId}/attachments")]
        public async Task<IActionResult> UploadAttachment(int contractorId, int projectId, int materialId, [FromForm] IFormFile file)
        {
            if (!await ContractorExistsAsync(contractorId))
            {
                return NotFound("Contractor not found.");
            }
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided or file is empty.");
            }
            try
            {
                var material = await _materialService.GetMaterialAsync(projectId, materialId);
                if (material == null)
                {
                    return NotFound("Material not found.");
                }

                var filePath = await _materialService.UploadAttachmentAsync(material, file);
                return Ok(new { Message = "File uploaded successfully.", FilePath = filePath });
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMaterial(int contractorId, int projectId, int materialId)
        {
            if (!await ContractorExistsAsync(contractorId))
            {
                return NotFound("Contractor not found.");
            }

            var isDeleted = await _materialService.DeleteMaterialAsync(projectId, materialId);
            if (!isDeleted)
            {
                return NotFound("Material not found.");
            }

            return NoContent();
        }
    }
}
