using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Utilizador
{
    public interface IUtilizadorInterface
    {
        Task<ResponseModel<UtilizadorModel>> GetUtilizarbyId(int idUtilizador);
        Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores();
    }
}