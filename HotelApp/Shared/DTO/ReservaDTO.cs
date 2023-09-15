using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Shared.DTO
{
	public class ReservaDTO 
	{
		public int NroReserva { get; set; }
		[Required(ErrorMessage = "La Fecha de inicio es Obligatoria")]
		public DateTime Fecha_inicio { get; set; }
		[Required(ErrorMessage = "La Fecha de fin es Obligatoria")]
		public DateTime Fecha_fin { get; set; }
		[Required(ErrorMessage = "El Dni del dueño de la reserva es obligatorio")]
		public int Dni { get; set; }
        [Required(ErrorMessage = "El Dni de los huespedes es obligatorio")]
        public List<string> Dns { get; set; } = new List<string>();
        [Required(ErrorMessage = "se requiere la lista de habitaciones")]
        public List<string> Nhabs { get; set; } = new List<string>();
        public List<HuespedDTO> Huespedes { get; set; } = new List<HuespedDTO>();

		public List<HabitacionDTO> Habitaciones { get; set; } = new List<HabitacionDTO>();
	}
}
