using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reservas.BData.Data.Entity
{
    public class Habitacion
	{
		public int Id { get; set; }
		public string Nhab { get; set; }

		[Required]
		public int Camas { get; set; }

		[Required(ErrorMessage = "El estado es Obligatorio")]
		public string Estado { get; set; } = "";

		[Required]
		public decimal Precio { get; set; }
		[Required]
		public decimal Garantia { get; set; }
		[NotMapped]
        //public int reservadidhab { get; set; }
		//[JsonIgnore]
        public Reserva Reserva { get; set; }
    }
}
