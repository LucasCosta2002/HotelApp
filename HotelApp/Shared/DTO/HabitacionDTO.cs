using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Shared.DTO
{
	public class HabitacionDTO
	{
		public string? Nhab { get; set; }

		public int? Camas { get; set; }
		public string? Estado { get; set; }
		public decimal? Precio { get; set; }
		public decimal? Garantia { get; set; }

	}
}
