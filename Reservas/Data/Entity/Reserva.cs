using System.ComponentModel.DataAnnotations;

namespace Reservas.BData.Data.Entity
{
    public class Reserva
	{
		public int Id { get; set; }
		[Required]
		public int NroReserva { get; set; }

        [Required(ErrorMessage = "La Fecha de inicio es Obligatoria")]
		public DateTime Fecha_inicio { get; set; }
		[Required(ErrorMessage = "La Fecha de fin es Obligatoria")]
		public DateTime Fecha_fin { get; set; }
		[Required(ErrorMessage = "El Dni del dueño de la reserva es obligatorio")]
		public int Dni { get; set; }
        [Required(ErrorMessage = "El Dni de los huespedes es obligatorio")]
        public string DniHuesped { get; set; }
        public List<Huesped> Huespedes { get; set; } = new List<Huesped>();
        [Required(ErrorMessage = "se requieren los numeros de habitaciones")]
        public string nhabs { get; set; }
        public List<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();	
    }

    
}
