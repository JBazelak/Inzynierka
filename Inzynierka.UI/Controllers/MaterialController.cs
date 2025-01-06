using Inzynierka.UI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inzynierka.UI.Controllers
{
    [Route("api/contractors/{contractorId}/projects/{projectId}/materials/{materialId}/attachments")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        private readonly IContractorService _contractorService;

        public MaterialController(IMaterialService materialService, IContractorService contractorService)
        {
            _materialService = materialService;
            _contractorService = contractorService;
        }

        private async Task<bool> ContractorExistsAsync(int contractorId)
        {
            return await _contractorService.ContractorExistsAsync(contractorId);
        }

        [HttpPost]
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
