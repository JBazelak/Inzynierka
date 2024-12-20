using AutoMapper;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inzynierka.UI.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MaterialController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetAll()
        {
            var materials = await _context.Materials.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<MaterialDto>>(materials));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MaterialDto>> GetById(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
                return NotFound();

            return Ok(_mapper.Map<MaterialDto>(material));
        }

        [HttpPost]
        public async Task<ActionResult> Create(MaterialDto materialDto)
        {
            var material = _mapper.Map<Material>(materialDto);
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<MaterialDto>(material);
            return CreatedAtAction(nameof(GetById), new { id = material.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, MaterialDto materialDto)
        {
            if (id != materialDto.Id)
                return BadRequest();

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
                return NotFound();

            _mapper.Map(materialDto, material);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
                return NotFound();

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}