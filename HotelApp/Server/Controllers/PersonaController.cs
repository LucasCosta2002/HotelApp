using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Persona")]
    //
    public class PersonaController : ControllerBase
    {
        private readonly Context context;

        public PersonaController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Persona>>> Get()
        {
            return await context.Personas.ToListAsync();
        }

        [HttpGet("int:dniPersona")]
        public async Task<ActionResult<Persona>> GetDniPersona(int dniPersona)
        {
            var buscar = await context.Personas.FirstOrDefaultAsync(c => c.Dni == dniPersona);

            if (buscar is null)
            {
                return BadRequest($"No se encontro la Persona de Dni numero: {dniPersona}");
            }

            return buscar;
        }
        [HttpPost]
        public async Task<IActionResult> Post(PersonaDTO personaDTO)
        {
            var entidad = await context.Personas.FirstOrDefaultAsync(x => x.Dni == personaDTO.Dni);

            if (entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe un persona con ese DNI");
            }

            try
            {
                var mdPersona = new Persona
                {
                    Dni = (int)personaDTO.Dni,
                    Nombres = personaDTO.Nombres,
                    Apellidos = personaDTO.Apellidos,
                    Correo = personaDTO.Correo,
                    Telefono = personaDTO.Telefono,
                    NumTarjeta = personaDTO.NumTarjeta

                };
                context.Personas.Add(mdPersona);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex); }
        }

        [HttpPut]
        public async Task<IActionResult> Editar(PersonaDTO personaDTO, int dniPer)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbPersona = await context.Personas.FirstOrDefaultAsync(e => e.Dni == dniPer);
                if (dbPersona != null)
                {
                    dbPersona.Nombres = personaDTO.Nombres;
                    dbPersona.Apellidos = personaDTO.Apellidos;
                    dbPersona.Correo = personaDTO.Correo;
                    dbPersona.Telefono = personaDTO.Telefono;
                    dbPersona.NumTarjeta = personaDTO.NumTarjeta;
                    context.Personas.Update(dbPersona);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbPersona.Dni;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Persona no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpDelete]
        public async Task<IActionResult> Borrar(int dniHuesp)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbPersona = await context.Personas.FirstOrDefaultAsync(e => e.Dni == dniHuesp);
                if (dbPersona != null)
                {
                    context.Personas.Remove(dbPersona);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                }else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Persona no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }
    }
}
