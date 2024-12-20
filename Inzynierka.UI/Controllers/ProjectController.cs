using AutoMapper;
using Inzynierka.Infrastructure.Persistance;
using Inzynierka.Core.Entities;
using Inzynierka.UI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Inzynierka.UI.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll()
        {
            var projects = await _context.Projects.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetById(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        [HttpPost]
        public async Task<ActionResult> Create(ProjectDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<ProjectDto>(project);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ProjectDto projectDto)
        {
            if (id != projectDto.Id)
                return BadRequest();

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            _mapper.Map(projectDto, project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}