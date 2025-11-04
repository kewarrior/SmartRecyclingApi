using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Operador
{
    public interface IOperadorInterface
    {
        Task<ResponseModel<ReciclagemModel>> InserirDadosReciclagem(ReciclagemModel reciclagem);
    }
}
