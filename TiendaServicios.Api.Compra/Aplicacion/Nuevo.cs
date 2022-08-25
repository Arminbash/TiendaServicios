using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Compra.Modelo;
using TiendaServicios.Api.Compra.Persistencia;

namespace TiendaServicios.Api.Compra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime? FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;

            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesion.Add(carritoSesion);
                var valor = await _contexto.SaveChangesAsync();

                if (valor == 0) throw new Exception("Errores en la insercion del carrito de compras");

                int id = carritoSesion.CarritoSesionId;

                foreach(var obj in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = obj
                    };
                    _contexto.CarritoSesionDetalle.Add(detalleSesion);
                }
                valor = await _contexto.SaveChangesAsync();

                if (valor > 0) return Unit.Value;

                throw new Exception("No se pudo guardar el detalle");

            }
        }
    }
}
