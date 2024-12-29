using Microsoft.AspNetCore.Mvc;
using Inzynierka.UI.DTOs;
using Inzynierka.UI.Interfaces;
using Inzynierka.UI.Filters;

[ApiController]
[Route("api/contractors")]
public class ContractorController : ControllerBase
{
    private readonly IContractorService _contractorService;

    public ContractorController(IContractorService contractorService)
    {
        _contractorService = contractorService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContractorDto>> GetById(int id)
    {
        try
        {
            var contractor = await _contractorService.GetByIdAsync(id);
            return Ok(contractor);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [SessionAuthorization]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateContractorDto updateContractorDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _contractorService.UpdateAsync(id, updateContractorDto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return Conflict(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [SessionAuthorization]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _contractorService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
