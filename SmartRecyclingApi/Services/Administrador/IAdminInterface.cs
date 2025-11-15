using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Services.Administrador
{
    public interface IAdminInterface
    {
        Task<ResponseModel<object>> UpdatePedido(long id);
        Task<ResponseModel<object>> ValidarPassword(UtilizadorCriacaoDTO verificarUtilizador);
    }
}