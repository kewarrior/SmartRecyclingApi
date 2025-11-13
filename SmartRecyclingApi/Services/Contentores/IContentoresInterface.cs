using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Contentores
{
    public interface IContentoresInterface
    {
        Task<ResponseModel<List<ReciclagemModel>>> GellAllDadosReciclagem();
        Task<ResponseModel<List<ReciclagemModel>>> GetDadosByUtilizadorId(long id);
    }
}
