using Microsoft.EntityFrameworkCore;
using Reservas.BData.Data.Entity;
using System.Reflection.Emit;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System.Diagnostics;

namespace Reservas.BData
{
	public class Context : DbContext
	{
		public Context() { }
		public Context(DbContextOptions<Context> options) : base(options) { }
		public DbSet<Huesped> Huespedes => Set<Huesped>();
		public DbSet<Habitacion> Habitaciones => Set<Habitacion>();
		public DbSet<Reserva> Reservas => Set<Reserva>();

		public DbSet<Persona> Personas => Set<Persona>();
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{ }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Habitacion>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.Nhab);
				o.Property(b => b.Camas);
				o.Property(b => b.Estado);
				o.Property(b => b.Precio).HasColumnType("Decimal(10,2)");
				o.Property(b => b.Garantia).HasColumnType("Decimal(10,2)");
			});

			modelBuilder.Entity<Persona>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.Dni);
				o.Property(b => b.Nombres);
				o.Property(b => b.Apellidos);
				o.Property(b => b.Correo);
				o.Property(b => b.Telefono);
				o.Property(b => b.NumTarjeta);
			});
			modelBuilder.Entity<Huesped>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.Dni);
				o.Property(b => b.Nombres);
				o.Property(b => b.Apellidos);
				o.Property(b => b.Checkin);
				o.Property(b => b.Num_Hab);
				o.Property(b => b.DniPersona);
			});
			modelBuilder.Entity<Reserva>(o =>
			{
				o.HasKey(b => b.Id);
				o.Property(b => b.NroReserva);
				o.Property(b => b.Fecha_inicio);
				o.Property(b => b.Fecha_fin);
				o.Property(b => b.Dni);
				o.Property(b => b.nhabs);
				o.HasMany(b => b.Huespedes);
				o.HasMany(b => b.Habitaciones);
			});



			//modelBuilder
			//.Entity<Habitacion>()
			//.HasOne(e => e.reservadidhab)
			//.OnDelete(DeleteBehavior.ClientCascade);



            
			modelBuilder.Entity<Habitacion>()
				.HasOne<Reserva>(s => s.Reserva)
				.WithMany(g => g.Habitaciones)
				.HasForeignKey("ReservadDeHabitacionId")//clabe foranea de propiedad semilla
				.OnDelete(DeleteBehavior.Cascade);


			//modelBuilder.Entity<Huesped>()
			//	.HasOne<Reserva>(s => s.reserva)
			//	.WithMany(g => g.Huespedes)
			//	.HasForeignKey("reservadidhuesp")
			//	.OnDelete(DeleteBehavior.Cascade);

        }

	}
}


//https://www.entityframeworktutorial.net/efcore/configure-one-to-many-relationship-using-fluent-api-in-ef-core.aspx-->ESTA HECHO EN WEBFORM(C# CON ASP.NET)

//https://learn.microsoft.com/es-es/ef/core/modeling/relationships/one-to-one

//https://learn.microsoft.com/en-us/ef/core/saving/cascade-delete

