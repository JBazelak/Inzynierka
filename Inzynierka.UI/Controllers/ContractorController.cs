using Microsoft.AspNetCore.Mvc;
using Inzynierka.UI.DTOs;
using Inzynierka.UI.Interfaces;

[ApiController]
[Route("api/contractors")]
public class ContractorController : ControllerBase
{
    private readonly IContractorService _contractorService;

    public ContractorController(IContractorService contractorService)
    {
        _contractorService = contractorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractorDto>>> GetAll()
    {
        var contractors = await _contractorService.GetAllAsync();
        return Ok(contractors);
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

    [HttpPost]
    public async Task<ActionResult<ContractorDto>> Create([FromBody] ContractorDto contractorDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdContractor = await _contractorService.CreateAsync(contractorDto);
        return CreatedAtAction(nameof(GetById), new { id = createdContractor.Id }, createdContractor);
    }

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
        catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentException)
        {
            return BadRequest(ex.Message);
        }
    }

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
