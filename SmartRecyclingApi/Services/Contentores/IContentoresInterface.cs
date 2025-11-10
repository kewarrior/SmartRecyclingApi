using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Contentores
{
    public interface IContentoresInterface
    {
        Task<ResponseModel<List<ReciclagemModel>>> GellAllDadosReciclagem();
    }
}
