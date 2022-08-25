using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Compra.Persistencia;
using TiendaServicios.Api.Compra.RemoteInterface;

namespace TiendaServicios.Api.Compra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto _contexto;
            private readonly ILibrosService _libroService;

            public Manejador(CarritoContexto contexto, ILibrosService libroService)
            {
                _contexto=contexto;
                _libroService=libroService;
            }

            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();
                foreach(var libro in carritoSesionDetalle)
                {
                   var response = await _libroService.GetLibro(new System.Guid(libro.ProductoSeleccionado));

                    if (response.resultado)
                    {
                        var objetoLibro = response.Libro;

                        var carritoDetalleDto = new CarritoDetalleDto
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                            LibroId =  objetoLibro.LibreriaMaterialId,
                            AutorLibro = objetoLibro.AutorLibro.ToString()
                        };

                        listaCarritoDto.Add(carritoDetalleDto);
                    }
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                };

                return carritoSesionDto;
            }
        }
    }
}
