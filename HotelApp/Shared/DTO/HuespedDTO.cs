using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Shared.DTO
{
	public class HuespedDTO
	{
		[Required(ErrorMessage = "El DNI es Obligatorio")]
		[MaxLength(10, ErrorMessage = "El Debe tener 10 caracteres maximo")]
		public string Dni { get; set; }
		public string Nombres { get; set; } = "";
		[Required(ErrorMessage = "El Apellido es Obligatorio")]
		[MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Apellido")]
		public string Apellidos { get; set; } = "";
		public bool Checkin { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "El campo {0} es requerido")]
		public int Num_Hab { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "El campo {0} es requerido")]
		public int DniPersona { get; set; }
	}
}
