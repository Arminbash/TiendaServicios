using System;
using System.Threading.Tasks;
using TiendaServicios.Api.Compra.RemoteModel;

namespace TiendaServicios.Api.Compra.RemoteInterface
{
    public interface ILibrosService
    {
        Task<(bool resultado, LibroRemote Libro, string ErrorMessage)> GetLibro(Guid LibroId);
    }
}
