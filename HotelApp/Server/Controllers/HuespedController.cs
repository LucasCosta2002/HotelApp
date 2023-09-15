using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Huesped")]
    //
    public class HuespedController : ControllerBase
    {
        private readonly Context context;
        public HuespedController(Context context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Huesped>>> Get()
        {
            return await context.Huespedes.ToListAsync();
        }

        [HttpGet("int:dniHuesped")]
        public async Task<ActionResult<Huesped>> GetDniPersona(string dniHuesped)
        {
            var buscar = await context.Huespedes.FirstOrDefaultAsync(c => c.Dni == dniHuesped);

            if (buscar is null)
            {
                return BadRequest($"No se encontro el huesped de Dni numero: {dniHuesped}");
            }
            return buscar;
        }
        [HttpPost]
        public async Task<IActionResult> Post(HuespedDTO HuespedDTO)
        {

            var entidad = await context.Huespedes.FirstOrDefaultAsync(x => x.Dni == HuespedDTO.Dni);

            if (entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe un Huesped con ese DNI");
            }

            try
            {
                var mdHuesped = new Huesped
                {
                    Dni = HuespedDTO.Dni,
                    Nombres = HuespedDTO.Nombres,
                    Apellidos = HuespedDTO.Apellidos,
                    Checkin = HuespedDTO.Checkin,
                    Num_Hab = HuespedDTO.Num_Hab,
                    DniPersona = HuespedDTO.DniPersona

                };
                context.Huespedes.Add(mdHuesped);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex); }
        }

        [HttpPut]
        public async Task <IActionResult> Modificar(HuespedDTO HuespedDTO, string dniHues)
        {
            var responseApi = new ResponseAPI<int>();
            try
            {
                var dbHuesped = await context.Huespedes.FirstOrDefaultAsync(e => e.Dni == dniHues);
                if (dbHuesped.Dni != "")
                {
                    dbHuesped.Nombres = HuespedDTO.Nombres;
                    dbHuesped.Apellidos = HuespedDTO.Apellidos;
                    dbHuesped.Checkin = HuespedDTO.Checkin;
                    dbHuesped.Num_Hab = HuespedDTO.Num_Hab;
                    dbHuesped.DniPersona = HuespedDTO.DniPersona;
                    context.Huespedes.Update(dbHuesped);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "se modifico el huesped de dni: " + dbHuesped.Dni;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "DNI no encontrado";
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
        public async Task <IActionResult> Borrar(string dniHuesped)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbHuesped = await context.Huespedes.FirstOrDefaultAsync(e => e.Dni == dniHuesped);
                if (dbHuesped.Dni != "")
                {
                    context.Huespedes.Remove(dbHuesped);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                } else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Huesped no encontrado";
                }
            }catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

    }
}
