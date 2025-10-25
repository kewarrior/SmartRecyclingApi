using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Utilizador
{
    public interface IUtilizadorInterface
    {
        Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores();
        Task<ResponseModel<UtilizadorModel>> GetUtilizarbyId(int idUtilizador);


    }
}