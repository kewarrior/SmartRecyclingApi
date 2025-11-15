using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Administrador
{
    public interface IAdminInterface
    {
        Task<ResponseModel<object>> UpdatePedido(long id);
    }
}
