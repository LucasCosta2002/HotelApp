using System.ComponentModel.DataAnnotations;

namespace Reservas.BData.Data.Entity
{
    public class Huesped
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "El DNI es Obligatorio")]
		public string? Dni { get; set; }
		[Required(ErrorMessage = "El Nombre es Obligatorio")]
		[MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Nombre")]
		public string Nombres { get; set; } = "";
		[Required(ErrorMessage = "El Apellido es Obligatorio")]
		[MaxLength(50, ErrorMessage = "Solo se aceptan hasta 50 caracteres en el Apellido")]
		public string Apellidos { get; set; } = "";
		public bool Checkin { get; set; }
		public int Num_Hab { get; set; }
		public int DniPersona { get; set; }
		//[NotMapped]
  //      //public int reservadidhuesp { get; set; }
		////[JsonIgnore]
  //      public Reserva reserva { get; set; }

    }
}