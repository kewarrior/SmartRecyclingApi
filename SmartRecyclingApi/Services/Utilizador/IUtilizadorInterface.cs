using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Services.Utilizador
{
    public interface IUtilizadorInterface
    {
        Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores();
        Task<ResponseModel<UtilizadorModel>> GetUtilizarbyId(int idUtilizador);
        Task<ResponseModel<UtilizadorModel>> CriarUtilizador(UtilizadorCriacaoDTO utilizadorCriacaoDto);
        Task<ResponseModel<UtilizadorModel>> EditarUtilizador(EditarUtilizadorDto editarUtilizadorDto);
        Task<ResponseModel<LoginResponseModel>> Login(LoginRequest request );

    }
}