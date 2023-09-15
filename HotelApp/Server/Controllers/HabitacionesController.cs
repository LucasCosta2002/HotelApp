using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Habitacion")]
    
    //

    public class HabitacionesController : ControllerBase
    {
        private readonly Context context;

        public HabitacionesController(Context dbcontext)
        {
            context = dbcontext;
        }


        [HttpGet]
        public async Task<ActionResult<List<Habitacion>>> Get()
        {
            var habitaciones = await context.Habitaciones.ToListAsync();
            return habitaciones;
        }

        [HttpGet("int:Id")]
        public async Task<ActionResult<Habitacion>> GetNroHabitacion(string nrohab)
        {
            var buscar = await context.Habitaciones.FirstOrDefaultAsync(c => c.Nhab==nrohab);

            if (buscar is null)
            {
                return BadRequest($"No se encontro la habitacion de numero: {nrohab}");
            }

            return buscar;
        }

        [HttpPost] 
        public async Task<IActionResult> Post(HabitacionDTO habitacionDTO)
        {

            var entidad = await context.Habitaciones.FirstOrDefaultAsync(x => x.Nhab == habitacionDTO.Nhab);

            if(entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe una Habitacion con ese número");
            }

            try {
                var mdHabitacion = new Habitacion
                {
                    Nhab = habitacionDTO.Nhab,
                    Camas = (int)habitacionDTO.Camas,
                    Estado = habitacionDTO.Estado,
                    Precio = (int)habitacionDTO.Precio,
                    Garantia = (int)habitacionDTO.Garantia
                };
                context.Habitaciones.Add(mdHabitacion);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpPut]

        public async Task<IActionResult> Editar(HabitacionDTO habitacionDTO, string nrohab)
        {
            var responseApi = new ResponseAPI<int>();

            try {
                var dbHabitacion = await context.Habitaciones.FirstOrDefaultAsync(e => e.Nhab == nrohab);
                if (dbHabitacion != null)
                {
                    dbHabitacion.Camas = (int)habitacionDTO.Camas;
                    dbHabitacion.Estado = habitacionDTO.Estado;
                    dbHabitacion.Precio = (decimal)habitacionDTO.Precio;
                    dbHabitacion.Garantia = (decimal)habitacionDTO.Garantia;
                    context.Habitaciones.Update(dbHabitacion);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Mensaje = "se cargo la hab" + dbHabitacion.Nhab;
                } else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "habitacion no encontrada";
                }
            }
            catch (Exception ex) {

                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.Message;
            }
            return Ok(responseApi);
        }

        [HttpDelete]

        public async Task<IActionResult> Delete(string nrohab)
        {
            var responseApi = new ResponseAPI<int>();

           try
            {
                var dbHabitacion = await context.Habitaciones.FirstOrDefaultAsync(e => e.Nhab == nrohab);
                if (dbHabitacion != null) 
                {
                    context.Habitaciones.Remove(dbHabitacion);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                }
                else { responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "habitacion no encontrada";
                }
            } catch (Exception ex) {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje =ex.Message;
            }
            return Ok(responseApi); 
        }

        [HttpPost("AgregarMuchasHabitaciones")]

        public async Task<ActionResult> PostHabitaciones(List<Habitacion> habitaciones)
        {
            context.AddRange(habitaciones);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}