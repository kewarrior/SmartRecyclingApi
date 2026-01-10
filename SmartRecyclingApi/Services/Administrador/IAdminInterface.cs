using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Services.Administrador
{
    public interface IAdminInterface
    {
        Task<ResponseModel<object>> UpdatePedido(long id);
        Task<ResponseModel<object>> ValidarPassword(LoginRequest verificarUtilizador);
        Task<ResponseModel<List<UtilizadorModel>>> Listar();
        Task<ResponseModel<UtilizadorModel>> EditarUtilizador(UtilizadorCriacaoDTO utilizador);
        Task<ResponseModel<UtilizadorModel>> ApagarUtilizador(long id);
        Task<ResponseModel<UtilizadorModel>> CriarADmin(UtilizadorCriacaoDTO adminUtilizador);
        Task<ResponseModel<UtilizadorModel>> CriarOperario(UtilizadorCriacaoDTO operarioUtilizador);

    }
}