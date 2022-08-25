using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Compra.Modelo;

namespace TiendaServicios.Api.Compra.Persistencia
{
    public class CarritoContexto : DbContext
    {
        public CarritoContexto(DbContextOptions<CarritoContexto> options) : base(options) { }

        public DbSet<CarritoSesion> CarritoSesion { get; set; }

        public DbSet<CarritoSesionDetalle> CarritoSesionDetalle { get; set; }
    }
}
