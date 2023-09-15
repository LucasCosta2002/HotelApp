using BlazorCrud.Shared;
using HotelApp.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservas.BData;
using Reservas.BData.Data.Entity;

namespace HotelApp.Server.Controllers
{
    [ApiController]
    [Route("api/Reservar")]
    //
    public class ReservaController : ControllerBase
    {
        private readonly Context context;

        public ReservaController(Context context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Reserva>>> Get()
        {
            return await context.Reservas.ToListAsync();
        }

        [HttpGet("int:NroReserva")]
        public async Task<ActionResult<Reserva>> GetDniPersona(int nroReserva)
        {
            var buscar = await context.Reservas.FirstOrDefaultAsync(c => c.NroReserva == nroReserva);

            if (buscar is null)
            {
                return BadRequest($"No se encontro la reserva de nro: {nroReserva}");
            }

            return buscar;
        }
        [HttpPost]
        public async Task<IActionResult> Post(ReservaDTO reservaDTO)
        {
            var responseApi = new ResponseAPI<int>(); 

            var entidad = await context.Reservas.FirstOrDefaultAsync(x => x.NroReserva == reservaDTO.NroReserva);

            if (entidad != null) // existe una hab con el num ingresado
            {
                return BadRequest("Ya existe una reserva");
            }


            try
            {
                var mdReserva = new Reserva
                {
                    NroReserva = reservaDTO.NroReserva,
                    Fecha_inicio = reservaDTO.Fecha_inicio,
                    Fecha_fin = reservaDTO.Fecha_fin,
                    Dni = reservaDTO.Dni,
                };
                responseApi.Mensaje += "se cargaron los huespedes de dni: ";
                foreach (var dnis in reservaDTO.Dns)
                {
                    var huesped = await context.Huespedes.FirstOrDefaultAsync(c => c.Dni == dnis);
                    if (huesped != null) { mdReserva.Huespedes.Add(huesped);
                        mdReserva.DniHuesped += dnis;
                        responseApi.Mensaje += huesped.Dni + ", ";
                    }
                    else { responseApi.EsCorrecto = false;
                        responseApi.Mensaje += " falta el huesped con dni " + dnis;
                    }
                }
                foreach (var habs in reservaDTO.Nhabs)
                {
                    var habitacion = await context.Habitaciones.FirstOrDefaultAsync(c => c.Nhab == habs);
                    if (habitacion != null)
                    {
                        mdReserva.Habitaciones.Add(habitacion);
                        mdReserva.nhabs += habs.ToString() + " , ";
                    } else { responseApi.EsCorrecto = false; responseApi.Mensaje += "fallo en la habitacion nro: " + habs; }
                }
                context.Reservas.Add(mdReserva);
                await context.SaveChangesAsync();
                return Ok(responseApi);
            }
            catch (Exception ex) { return BadRequest(ex.InnerException.Message); }
        }
        [HttpPut]
        public async Task<IActionResult> Editar(ReservaDTO reservaDTO, int nres)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                var dbReserva = await context.Reservas.FirstOrDefaultAsync(e => e.NroReserva == nres);

                if (dbReserva != null)
                {
                    dbReserva.NroReserva = reservaDTO.NroReserva;
                    dbReserva.Fecha_inicio = reservaDTO.Fecha_inicio;
                    dbReserva.Fecha_fin = reservaDTO.Fecha_fin;
                    dbReserva.Dni = reservaDTO.Dni;
                    dbReserva.DniHuesped = "";
                    dbReserva.nhabs = "";
                    foreach(var Huesped in dbReserva.Huespedes) { dbReserva.Huespedes.Remove(Huesped); }
                    foreach (var Habitacion in dbReserva.Habitaciones) { dbReserva.Habitaciones.Remove(Habitacion); }
                    foreach (var dnis in reservaDTO.Dns)
                    {
                        var huesped = await context.Huespedes.FirstOrDefaultAsync(c => c.Dni == dnis);
                        if (huesped != null)
                        {
                            dbReserva.Huespedes.Add(huesped);
                            dbReserva.DniHuesped += ", " + dnis;
                            responseApi.Mensaje += ", " + huesped.Dni;
                        }
                        else
                        {
                            responseApi.EsCorrecto = false;
                            responseApi.Mensaje += " falta el huesped con dni " + dnis;
                        }
                    }
                    foreach (var habs in reservaDTO.Nhabs)
                    {
                        var habitacion = await context.Habitaciones.FirstOrDefaultAsync(c => c.Nhab == habs);
                        if (habitacion != null)
                        {
                            dbReserva.Habitaciones.Add(habitacion);
                            dbReserva.nhabs += habs.ToString() + " , ";
                        }
                        else { responseApi.EsCorrecto = false; responseApi.Mensaje += "fallo en la habitacion nro: " + habs; }
                    }
                    context.Reservas.Update(dbReserva);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                    responseApi.Valor = dbReserva.NroReserva;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "reserva no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.InnerException.Message;
            }
            return Ok(responseApi);
        }
        [HttpDelete]
        public async Task<IActionResult> Borrar(int nroRes)
        {
            var responseApi = new ResponseAPI<int>();

            try
            {
                //var dbReserva = await context.Reservas.FirstOrDefaultAsync(e => e.NroReserva == nroRes);
                var dbReserva = await context.Reservas
                    .Include(c => c.Habitaciones)
                    .Include(c => c.Huespedes)
                    .FirstOrDefaultAsync(e => e.NroReserva == nroRes);

                if (dbReserva != null)
                {
                    context.Reservas.Remove(dbReserva);
                    await context.SaveChangesAsync();
                    responseApi.EsCorrecto = true;
                }
                else
                {
                    responseApi.EsCorrecto = false;
                    responseApi.Mensaje = "Reserva no encontrada";
                }
            }
            catch (Exception ex)
            {
                responseApi.EsCorrecto = false;
                responseApi.Mensaje = ex.InnerException.Message;
            }
            return Ok(responseApi);
        }
    }
}