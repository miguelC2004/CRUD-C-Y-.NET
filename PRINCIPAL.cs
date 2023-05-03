using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class Persona
{
    public int IdPersona { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Direccion { get; set; }
}

public class MotivoCita
{
    public int IdMotivo { get; set; }
    public string Descripcion { get; set; }
}

public class Cita
{
    public int IdCita { get; set; }
    public DateTime FechaCita { get; set; }
    public int IdPersona { get; set; }
    public Persona Persona { get; set; }
    public List<MotivoCita> MotivosCita { get; set; }
}


public class ClinicaContext : DbContext
{
    public ClinicaContext(DbContextOptions<ClinicaContext> options) : base(options)
    {

    }

    public DbSet<Persona> Personas { get; set; }
    public DbSet<MotivoCita> MotivosCita { get; set; }
    public DbSet<Cita> Citas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CitaMotivo>()
            .HasKey(cm => new { cm.IdCita, cm.IdMotivo });

        modelBuilder.Entity<CitaMotivo>()
            .HasOne(cm => cm.Cita)
            .WithMany(c => c.MotivosCita)
            .HasForeignKey(cm => cm.IdCita);

        modelBuilder.Entity<CitaMotivo>()
            .HasOne(cm => cm.MotivoCita)
            .WithMany(m => m.Citas)
            .HasForeignKey(cm => cm.IdMotivo);
    }
}

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly ClinicaContext _context;

    public
PersonasController(ClinicaContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<ActionResult<Persona>> PostPersona(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersona), new { id = persona.IdPersona }, persona);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersona(int id, Persona persona)
    {
        if (id != persona.IdPersona)
        {
            return BadRequest();
        }

        _context.Entry(persona).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool PersonaExists(int id)
    {
        return _context.Personas.Any(e => e.IdPersona == id);
    }

    [HttpPost("{idPersona}/Citas")]
    public async Task<ActionResult<Cita>> PostCita(int idPersona, Cita cita)
    {
        cita.IdPersona = idPersona;

        _context.Citas.Add(cita);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCita), new { id = cita.IdCita }, cita);
    }

    [HttpPut("{idCita}")]
    public async Task<IActionResult> PutCita(int idCita, Cita cita)
    {
        if (idCita != cita.IdCita)
        {
            return BadRequest();
        }

        _context.Entry(cita).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CitaExists(idCita))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    private bool CitaExists(int id)
    {
        return _context.Citas.Any(e => e.IdCita == id);
    }

